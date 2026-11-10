using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArenaHub.Application.Equipos;
using ArenaHub.Application.Inscripciones;
using ArenaHub.Application.Jugadores;
using ArenaHub.Application.Partidas;
using ArenaHub.Application.Ranking;
using ArenaHub.Application.Torneos;
using ArenaHub.Domain.Torneos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace ArenaHub.Tests;

// Recorre el flujo del enunciado contra la API real, con SQLite en memoria.
public sealed class FlujoCompletoTests : IDisposable
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly SqliteConnection _conexion;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public FlujoCompletoTests()
    {
        var cadena = $"Data Source=arenahub-{Guid.NewGuid()};Mode=Memory;Cache=Shared";
        _conexion = new SqliteConnection(cadena);
        _conexion.Open(); // la base en memoria vive mientras haya una conexión abierta

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.UseSetting("ConnectionStrings:ArenaHub", cadena));
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Torneo_de_4_equipos_termina_con_campeon_y_ratings_actualizados()
    {
        // 1. Cuatro equipos de cuatro jugadores
        var equipos = new List<int>();
        for (var e = 1; e <= 4; e++)
        {
            var equipo = await Post<EquipoDto>("/api/equipos", new CrearEquipoRequest($"Equipo {e}"));
            for (var j = 1; j <= 4; j++)
            {
                var jugador = await Post<JugadorDto>("/api/jugadores", new CrearJugadorRequest($"jugador-{e}-{j}"));
                await Post<EquipoDto>($"/api/equipos/{equipo.Id}/jugadores", new AgregarJugadorRequest(jugador.Id),
                    HttpStatusCode.OK);
            }
            equipos.Add(equipo.Id);
        }

        // 2 y 3. Torneo de cupo 4, inscripción y comienzo
        var torneo = await Post<TorneoDto>("/api/torneos", new CrearTorneoRequest("Copa ArenaHub", 4));
        await Transicionar(torneo.Id, EstadoTorneo.InscripcionAbierta);
        foreach (var equipoId in equipos)
            await Post<InscripcionDto>($"/api/torneos/{torneo.Id}/inscripciones", new InscribirEquipoRequest(equipoId));
        await Transicionar(torneo.Id, EstadoTorneo.EnCurso);

        // 4. Todos con 1000: la siembra desempata por inscripción, entonces 1 vs 4 y 2 vs 3
        var ronda1 = await Partidas(torneo.Id);
        Assert.Equal((equipos[0], equipos[3]), (ronda1[0].EquipoAId, ronda1[0].EquipoBId));
        Assert.Equal((equipos[1], equipos[2]), (ronda1[1].EquipoAId, ronda1[1].EquipoBId));

        // 5. Ganan 1 y 3: se genera la final
        await Resultado(ronda1[0].Id, equipos[0]);
        await Resultado(ronda1[1].Id, equipos[2]);
        var final = Assert.Single(await Partidas(torneo.Id), p => p.Ronda == 2);
        Assert.Equal((equipos[0], equipos[2]), (final.EquipoAId, final.EquipoBId));

        // 6. La final cierra el torneo
        await Resultado(final.Id, equipos[2]);
        var finalizado = await _client.GetFromJsonAsync<TorneoDto>($"/api/torneos/{torneo.Id}", Json);
        Assert.Equal(EstadoTorneo.Finalizado, finalizado!.Estado);
        Assert.Equal(equipos[2], finalizado.CampeonId);

        // 7. Ranking: todos arrancan con K=40 y ratings iguales, cada partida mueve 20 puntos
        var ranking = await _client.GetFromJsonAsync<List<PosicionDto>>("/api/ranking", Json);
        Assert.Equal(
            [(equipos[2], 1040, 2), (equipos[0], 1000, 2), (equipos[1], 980, 1), (equipos[3], 980, 1)],
            ranking!.Select(p => (p.EquipoId, p.Rating, p.SeriesJugadas)));

        // Una partida resuelta no se puede volver a cargar
        var repetida = await _client.PostAsJsonAsync($"/api/partidas/{final.Id}/resultado",
            new RegistrarResultadoRequest(equipos[0]));
        Assert.Equal(HttpStatusCode.Conflict, repetida.StatusCode);
        Assert.Contains("\"errorCode\":\"Partida.YaResuelta\"", await repetida.Content.ReadAsStringAsync());
    }

    private async Task<T> Post<T>(string url, object cuerpo, HttpStatusCode esperado = HttpStatusCode.Created)
    {
        var respuesta = await _client.PostAsJsonAsync(url, cuerpo, Json);
        Assert.Equal(esperado, respuesta.StatusCode);
        return (await respuesta.Content.ReadFromJsonAsync<T>(Json))!;
    }

    private Task<TorneoDto> Transicionar(int torneoId, EstadoTorneo destino) =>
        Post<TorneoDto>($"/api/torneos/{torneoId}/transiciones", new TransicionRequest(destino), HttpStatusCode.OK);

    private Task<PartidaDto> Resultado(int partidaId, int ganadorId) =>
        Post<PartidaDto>($"/api/partidas/{partidaId}/resultado", new RegistrarResultadoRequest(ganadorId),
            HttpStatusCode.OK);

    private async Task<List<PartidaDto>> Partidas(int torneoId) =>
        (await _client.GetFromJsonAsync<List<PartidaDto>>($"/api/torneos/{torneoId}/partidas", Json))!;

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        _conexion.Dispose();
    }
}

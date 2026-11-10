using ArenaHub.Application.Jugadores;
using ArenaHub.Domain.Equipos;

namespace ArenaHub.Application.Equipos;

public sealed record CrearEquipoRequest(string Nombre);

public sealed record AgregarJugadorRequest(int JugadorId);

public sealed record EquipoDto(int Id, string Nombre, int Rating, int SeriesJugadas, bool EsValido,
    IReadOnlyList<JugadorDto> Jugadores)
{
    internal static EquipoDto Desde(Equipo equipo) =>
        new(equipo.Id, equipo.Nombre, equipo.Rating, equipo.SeriesJugadas, equipo.EsValido,
            equipo.Jugadores.Select(JugadorDto.Desde).ToList());
}

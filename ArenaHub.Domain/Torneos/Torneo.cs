using ArenaHub.Domain.Common;
using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Torneos.Emparejamientos;
using ArenaHub.Domain.Torneos.Estados;
using ArenaHub.Domain.Torneos.Inscripciones;

namespace ArenaHub.Domain.Torneos;

public sealed class Torneo
{
    private static readonly int[] CuposPermitidos = [4, 8, 16];

    private readonly List<Inscripcion> _inscripciones = [];
    private readonly List<Partida> _partidas = [];

    private Torneo(string nombre, int cupo)
    {
        Nombre = nombre;
        Cupo = cupo;
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public int Cupo { get; private set; }
    public EstadoTorneo Estado { get; private set; } = EstadoTorneo.Borrador;
    public int? CampeonId { get; private set; }
    public IReadOnlyCollection<Inscripcion> Inscripciones => _inscripciones;
    public IReadOnlyCollection<Partida> Partidas => _partidas;

    public bool CupoCompleto => _inscripciones.Count == Cupo;

    public static Result<Torneo> Crear(string nombre, int cupo) =>
        CuposPermitidos.Contains(cupo) ? new Torneo(nombre, cupo) : TorneoErrores.CupoInvalido(cupo);

    public Result PasarA(EstadoTorneo destino) => EstadoDelTorneo.De(Estado).PasarA(this, destino);

    public Result<Inscripcion> Inscribir(Equipo equipo)
    {
        var reglas = ReglasDeInscripcion.Evaluar(new SolicitudDeInscripcion(this, equipo));
        if (reglas.IsFailure) return reglas.Error!;

        var inscripcion = new Inscripcion(equipo, DateTime.UtcNow);
        _inscripciones.Add(inscripcion);
        return inscripcion;
    }

    public Result<Partida> ResolverPartida(int partidaId, int ganadorId)
    {
        var partida = _partidas.FirstOrDefault(p => p.Id == partidaId);
        if (partida is null) return PartidaErrores.NoEncontrada(partidaId);

        var resolucion = partida.Resolver(ganadorId);
        if (resolucion.IsFailure) return resolucion.Error!;

        var perdedorId = ganadorId == partida.EquipoAId ? partida.EquipoBId : partida.EquipoAId;
        Elo.AplicarResultado(EquipoInscripto(ganadorId), EquipoInscripto(perdedorId));
        AvanzarBracket(partida.Ronda);
        return partida;
    }

    internal void AbrirInscripcion() => Estado = EstadoTorneo.InscripcionAbierta;

    internal void Iniciar()
    {
        var sembrados = _inscripciones
            .OrderByDescending(i => i.Equipo.Rating)
            .ThenBy(i => i.FechaInscripcion)
            .Select(i => i.EquipoId)
            .ToList();

        GenerarRonda(1, sembrados, new EmparejamientoPorSiembra());
        Estado = EstadoTorneo.EnCurso;
    }

    private void AvanzarBracket(int ronda)
    {
        var partidasDeLaRonda = _partidas.Where(p => p.Ronda == ronda).OrderBy(p => p.Numero).ToList();
        if (partidasDeLaRonda.Any(p => !p.Resuelta)) return;

        var ganadores = partidasDeLaRonda.Select(p => p.GanadorId!.Value).ToList();
        if (ganadores.Count == 1)
        {
            CampeonId = ganadores[0];
            Estado = EstadoTorneo.Finalizado;
            return;
        }

        GenerarRonda(ronda + 1, ganadores, new EmparejamientoDeGanadores());
    }

    private void GenerarRonda(int ronda, IReadOnlyList<int> equipos, IEstrategiaDeEmparejamiento estrategia)
    {
        var cruces = estrategia.Emparejar(equipos);
        for (var i = 0; i < cruces.Count; i++)
            _partidas.Add(new Partida(ronda, i + 1, cruces[i].EquipoAId, cruces[i].EquipoBId));
    }

    private Equipo EquipoInscripto(int equipoId) => _inscripciones.First(i => i.EquipoId == equipoId).Equipo;
}

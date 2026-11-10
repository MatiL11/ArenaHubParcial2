using ArenaHub.Domain.Equipos;

namespace ArenaHub.Domain.Torneos;

public sealed class Inscripcion
{
    private Inscripcion() { }

    internal Inscripcion(Equipo equipo, DateTime fechaInscripcion)
    {
        Equipo = equipo;
        EquipoId = equipo.Id;
        FechaInscripcion = fechaInscripcion;
    }

    public int Id { get; private set; }
    public int TorneoId { get; private set; }
    public int EquipoId { get; private set; }
    public Equipo Equipo { get; private set; } = null!;
    public DateTime FechaInscripcion { get; private set; }
}

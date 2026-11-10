using ArenaHub.Domain.Common;
using ArenaHub.Domain.Equipos;

namespace ArenaHub.Domain.Torneos;

public static class InscripcionErrores
{
    public static readonly Error TorneoCerrado =
        Error.Conflict("Inscripcion.TorneoCerrado", "El torneo no tiene la inscripción abierta.");

    public static readonly Error EquipoInvalido =
        Error.Conflict("Inscripcion.EquipoInvalido",
            $"El equipo debe tener entre {Equipo.MinimoDeJugadores} y {Equipo.MaximoDeJugadores} jugadores.");

    public static readonly Error Duplicada =
        Error.Conflict("Inscripcion.Duplicada", "El equipo ya está inscripto en el torneo.");

    public static readonly Error CupoCompleto =
        Error.Conflict("Inscripcion.CupoCompleto", "El torneo ya completó su cupo.");
}

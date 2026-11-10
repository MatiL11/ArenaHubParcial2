using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Inscripciones;

public sealed record InscribirEquipoRequest(int EquipoId);

public sealed record InscripcionDto(int EquipoId, string Equipo, int Rating, DateTime FechaInscripcion)
{
    internal static InscripcionDto Desde(Inscripcion inscripcion) =>
        new(inscripcion.EquipoId, inscripcion.Equipo.Nombre, inscripcion.Equipo.Rating, inscripcion.FechaInscripcion);
}

using ArenaHub.Domain.Common;
using ArenaHub.Domain.Equipos;

namespace ArenaHub.Domain.Torneos.Inscripciones;

internal sealed record SolicitudDeInscripcion(Torneo Torneo, Equipo Equipo);

// RN-06: se evalúan en orden y la primera que falla corta con su error.
internal static class ReglasDeInscripcion
{
    private static readonly Especificacion<SolicitudDeInscripcion> Todas =
        new TorneoConInscripcionAbierta()
            .Y(new EquipoValido())
            .Y(new EquipoNoInscripto())
            .Y(new CupoDisponible());

    public static Result Evaluar(SolicitudDeInscripcion solicitud) => Todas.Evaluar(solicitud);
}

internal sealed class TorneoConInscripcionAbierta : Especificacion<SolicitudDeInscripcion>
{
    public override Result Evaluar(SolicitudDeInscripcion s) =>
        s.Torneo.Estado == EstadoTorneo.InscripcionAbierta ? Result.Success() : InscripcionErrores.TorneoCerrado;
}

internal sealed class EquipoValido : Especificacion<SolicitudDeInscripcion>
{
    public override Result Evaluar(SolicitudDeInscripcion s) =>
        s.Equipo.EsValido ? Result.Success() : InscripcionErrores.EquipoInvalido;
}

internal sealed class EquipoNoInscripto : Especificacion<SolicitudDeInscripcion>
{
    public override Result Evaluar(SolicitudDeInscripcion s) =>
        s.Torneo.Inscripciones.Any(i => i.EquipoId == s.Equipo.Id) ? InscripcionErrores.Duplicada : Result.Success();
}

internal sealed class CupoDisponible : Especificacion<SolicitudDeInscripcion>
{
    public override Result Evaluar(SolicitudDeInscripcion s) =>
        s.Torneo.CupoCompleto ? InscripcionErrores.CupoCompleto : Result.Success();
}

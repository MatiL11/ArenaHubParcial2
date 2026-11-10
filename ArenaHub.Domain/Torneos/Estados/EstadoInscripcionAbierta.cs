using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos.Estados;

internal sealed class EstadoInscripcionAbierta : EstadoDelTorneo
{
    public override EstadoTorneo Actual => EstadoTorneo.InscripcionAbierta;

    public override Result PasarA(Torneo torneo, EstadoTorneo destino)
    {
        if (destino != EstadoTorneo.EnCurso) return base.PasarA(torneo, destino);
        if (!torneo.CupoCompleto) return TorneoErrores.CupoIncompleto(torneo.Inscripciones.Count, torneo.Cupo);

        torneo.Iniciar();
        return Result.Success();
    }
}

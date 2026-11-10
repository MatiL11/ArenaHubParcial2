using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos.Estados;

internal sealed class EstadoEnCurso : EstadoDelTorneo
{
    public override EstadoTorneo Actual => EstadoTorneo.EnCurso;

    // A Finalizado se llega solo al resolver la final (RN-11), nunca a mano.
    public override Result PasarA(Torneo torneo, EstadoTorneo destino) =>
        destino == EstadoTorneo.Finalizado ? TorneoErrores.FinalSinResolver : base.PasarA(torneo, destino);
}

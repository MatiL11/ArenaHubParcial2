using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos.Estados;

internal sealed class EstadoBorrador : EstadoDelTorneo
{
    public override EstadoTorneo Actual => EstadoTorneo.Borrador;

    public override Result PasarA(Torneo torneo, EstadoTorneo destino)
    {
        if (destino != EstadoTorneo.InscripcionAbierta) return base.PasarA(torneo, destino);

        torneo.AbrirInscripcion();
        return Result.Success();
    }
}

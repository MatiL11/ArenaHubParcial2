namespace ArenaHub.Domain.Torneos.Estados;

internal sealed class EstadoFinalizado : EstadoDelTorneo
{
    public override EstadoTorneo Actual => EstadoTorneo.Finalizado;
}

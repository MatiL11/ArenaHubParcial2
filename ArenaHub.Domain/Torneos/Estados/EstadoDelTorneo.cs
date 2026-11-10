using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos.Estados;

internal abstract class EstadoDelTorneo
{
    public static EstadoDelTorneo De(EstadoTorneo estado) => estado switch
    {
        EstadoTorneo.Borrador => new EstadoBorrador(),
        EstadoTorneo.InscripcionAbierta => new EstadoInscripcionAbierta(),
        EstadoTorneo.EnCurso => new EstadoEnCurso(),
        _ => new EstadoFinalizado()
    };

    public abstract EstadoTorneo Actual { get; }

    // Por defecto ninguna transición está permitida: cada estado habilita solo la suya.
    public virtual Result PasarA(Torneo torneo, EstadoTorneo destino) =>
        TorneoErrores.TransicionInvalida(Actual, destino);
}

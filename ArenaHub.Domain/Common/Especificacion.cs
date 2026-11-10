namespace ArenaHub.Domain.Common;

public abstract class Especificacion<T>
{
    public abstract Result Evaluar(T candidato);

    public Especificacion<T> Y(Especificacion<T> otra) => new EspecificacionY(this, otra);

    private sealed class EspecificacionY(Especificacion<T> primera, Especificacion<T> segunda) : Especificacion<T>
    {
        public override Result Evaluar(T candidato)
        {
            var resultado = primera.Evaluar(candidato);
            return resultado.IsFailure ? resultado : segunda.Evaluar(candidato);
        }
    }
}

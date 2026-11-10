namespace ArenaHub.Domain.Torneos.Emparejamientos;

// Rondas siguientes: ganador de la partida 1 vs ganador de la 2, y así sucesivamente.
public sealed class EmparejamientoDeGanadores : IEstrategiaDeEmparejamiento
{
    public IReadOnlyList<Cruce> Emparejar(IReadOnlyList<int> ganadores) =>
        ganadores.Chunk(2).Select(par => new Cruce(par[0], par[1])).ToList();
}

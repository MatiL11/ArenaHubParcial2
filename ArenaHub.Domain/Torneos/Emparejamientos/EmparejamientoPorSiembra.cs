namespace ArenaHub.Domain.Torneos.Emparejamientos;

// Primera ronda: recibe los equipos ya ordenados por siembra (el primero es el de mejor rating).
public sealed class EmparejamientoPorSiembra : IEstrategiaDeEmparejamiento
{
    public IReadOnlyList<Cruce> Emparejar(IReadOnlyList<int> sembrados) =>
        OrdenDelCuadro(sembrados.Count)
            .Chunk(2)
            .Select(par => new Cruce(sembrados[par[0] - 1], sembrados[par[1] - 1]))
            .ToList();

    // Cada siembra s se cruza con n + 1 - s: [1] → [1,2] → [1,4,2,3] → [1,8,4,5,2,7,3,6].
    private static List<int> OrdenDelCuadro(int cantidad)
    {
        List<int> orden = [1];
        while (orden.Count < cantidad)
        {
            var n = orden.Count * 2;
            orden = orden.SelectMany(s => new[] { s, n + 1 - s }).ToList();
        }
        return orden;
    }
}

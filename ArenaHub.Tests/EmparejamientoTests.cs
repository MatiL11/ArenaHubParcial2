using ArenaHub.Domain.Torneos.Emparejamientos;

namespace ArenaHub.Tests;

public class EmparejamientoTests
{
    // Los ids coinciden con la siembra: el equipo 1 es el de mejor rating.
    private static IReadOnlyList<int> Sembrados(int cantidad) => Enumerable.Range(1, cantidad).ToList();

    private static Cruce[] Cruces(params (int A, int B)[] pares) => pares.Select(p => new Cruce(p.A, p.B)).ToArray();

    [Fact]
    public void Siembra_de_4_equipos() =>
        Assert.Equal(Cruces((1, 4), (2, 3)), new EmparejamientoPorSiembra().Emparejar(Sembrados(4)));

    [Fact]
    public void Siembra_de_8_equipos() =>
        Assert.Equal(Cruces((1, 8), (4, 5), (2, 7), (3, 6)), new EmparejamientoPorSiembra().Emparejar(Sembrados(8)));

    [Fact]
    public void Siembra_de_16_equipos() =>
        Assert.Equal(
            Cruces((1, 16), (8, 9), (4, 13), (5, 12), (2, 15), (7, 10), (3, 14), (6, 11)),
            new EmparejamientoPorSiembra().Emparejar(Sembrados(16)));

    [Fact]
    public void Ganadores_se_cruzan_en_orden_del_bracket() =>
        Assert.Equal(Cruces((1, 5), (2, 7)), new EmparejamientoDeGanadores().Emparejar([1, 5, 2, 7]));
}

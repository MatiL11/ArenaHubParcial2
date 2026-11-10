using ArenaHub.Domain.Equipos;

namespace ArenaHub.Tests;

public class EloTests
{
    private static (int A, int B) Jugar(int ratingA, int seriesA, int ratingB, int seriesB, bool ganaA) =>
        (Elo.NuevoRating(ratingA, seriesA, ratingB, gano: ganaA),
         Elo.NuevoRating(ratingB, seriesB, ratingA, gano: !ganaA));

    [Fact]
    public void E1_ratings_iguales_gana_A() =>
        Assert.Equal((1020, 980), Jugar(1000, 0, 1000, 0, ganaA: true));

    [Fact]
    public void E2_gana_el_de_menor_rating() =>
        Assert.Equal((1515, 1685), Jugar(1500, 15, 1700, 15, ganaA: true));

    [Fact]
    public void E3_factores_K_distintos_dan_puntos_asimetricos() =>
        Assert.Equal((1185, 1030), Jugar(1200, 12, 1000, 3, ganaA: false));

    [Theory]
    [InlineData(0, 40)]
    [InlineData(9, 40)]
    [InlineData(10, 20)]
    public void Factor_K_segun_series_jugadas(int series, int esperado) =>
        Assert.Equal(esperado, Elo.FactorK(series));
}

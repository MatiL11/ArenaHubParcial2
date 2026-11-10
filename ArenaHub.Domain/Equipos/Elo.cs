namespace ArenaHub.Domain.Equipos;

public static class Elo
{
    public static int FactorK(int seriesJugadas) => seriesJugadas < 10 ? 40 : 20;

    public static double ResultadoEsperado(int rating, int ratingRival) =>
        1 / (1 + Math.Pow(10, (ratingRival - rating) / 400.0));

    public static int NuevoRating(int rating, int seriesJugadas, int ratingRival, bool gano)
    {
        var puntos = FactorK(seriesJugadas) * ((gano ? 1 : 0) - ResultadoEsperado(rating, ratingRival));
        return (int)Math.Round(rating + puntos, MidpointRounding.AwayFromZero);
    }

    // Los dos ratings nuevos se calculan con los valores previos a la partida.
    internal static void AplicarResultado(Equipo ganador, Equipo perdedor)
    {
        var ratingGanador = NuevoRating(ganador.Rating, ganador.SeriesJugadas, perdedor.Rating, gano: true);
        var ratingPerdedor = NuevoRating(perdedor.Rating, perdedor.SeriesJugadas, ganador.Rating, gano: false);

        ganador.RegistrarSerie(ratingGanador);
        perdedor.RegistrarSerie(ratingPerdedor);
    }
}

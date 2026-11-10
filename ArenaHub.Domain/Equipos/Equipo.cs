using ArenaHub.Domain.Common;
using ArenaHub.Domain.Jugadores;

namespace ArenaHub.Domain.Equipos;

public sealed class Equipo
{
    public const int MinimoDeJugadores = 3;
    public const int MaximoDeJugadores = 5;
    public const int RatingInicial = 1000;

    private readonly List<Jugador> _jugadores = [];

    public Equipo(string nombre) => Nombre = nombre;

    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public int Rating { get; private set; } = RatingInicial;
    public int SeriesJugadas { get; private set; }
    public IReadOnlyCollection<Jugador> Jugadores => _jugadores;

    public bool EsValido => _jugadores.Count is >= MinimoDeJugadores and <= MaximoDeJugadores;

    public Result AgregarJugador(Jugador jugador)
    {
        if (jugador.EquipoId is not null) return JugadorErrores.YaTieneEquipo(jugador.Nickname);
        if (_jugadores.Count >= MaximoDeJugadores) return EquipoErrores.Completo(Nombre);

        _jugadores.Add(jugador);
        jugador.AsignarEquipo(Id);
        return Result.Success();
    }

    internal void RegistrarSerie(int nuevoRating)
    {
        Rating = nuevoRating;
        SeriesJugadas++;
    }
}

using ArenaHub.Domain.Jugadores;

namespace ArenaHub.Application.Jugadores;

public sealed record CrearJugadorRequest(string Nickname);

public sealed record JugadorDto(int Id, string Nickname, int? EquipoId)
{
    internal static JugadorDto Desde(Jugador jugador) => new(jugador.Id, jugador.Nickname, jugador.EquipoId);
}

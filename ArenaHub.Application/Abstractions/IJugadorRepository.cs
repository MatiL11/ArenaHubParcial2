using ArenaHub.Domain.Jugadores;

namespace ArenaHub.Application.Abstractions;

public interface IJugadorRepository
{
    Task<Jugador?> ObtenerAsync(int id, CancellationToken ct);
    Task<bool> ExisteNicknameAsync(string nickname, CancellationToken ct);
    void Agregar(Jugador jugador);
}

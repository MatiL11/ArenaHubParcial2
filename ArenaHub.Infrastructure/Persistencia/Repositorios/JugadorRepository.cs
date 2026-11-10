using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Jugadores;
using Microsoft.EntityFrameworkCore;

namespace ArenaHub.Infrastructure.Persistencia.Repositorios;

internal sealed class JugadorRepository(ArenaHubDbContext db) : IJugadorRepository
{
    public Task<Jugador?> ObtenerAsync(int id, CancellationToken ct) =>
        db.Jugadores.FirstOrDefaultAsync(j => j.Id == id, ct);

    public Task<bool> ExisteNicknameAsync(string nickname, CancellationToken ct) =>
        db.Jugadores.AnyAsync(j => j.Nickname == nickname, ct);

    public void Agregar(Jugador jugador) => db.Jugadores.Add(jugador);
}

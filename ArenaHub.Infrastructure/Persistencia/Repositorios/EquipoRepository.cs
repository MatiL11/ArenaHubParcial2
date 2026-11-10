using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Equipos;
using Microsoft.EntityFrameworkCore;

namespace ArenaHub.Infrastructure.Persistencia.Repositorios;

internal sealed class EquipoRepository(ArenaHubDbContext db) : IEquipoRepository
{
    public Task<Equipo?> ObtenerAsync(int id, CancellationToken ct) =>
        db.Equipos.Include(e => e.Jugadores).FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<Equipo>> ListarPorRatingAsync(CancellationToken ct) =>
        await db.Equipos.AsNoTracking()
            .OrderByDescending(e => e.Rating)
            .ThenBy(e => e.Nombre)
            .ToListAsync(ct);

    public void Agregar(Equipo equipo) => db.Equipos.Add(equipo);
}

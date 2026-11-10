using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Torneos;
using Microsoft.EntityFrameworkCore;

namespace ArenaHub.Infrastructure.Persistencia.Repositorios;

internal sealed class TorneoRepository(ArenaHubDbContext db) : ITorneoRepository
{
    // El torneo se carga entero porque las reglas del bracket y del Elo necesitan inscripciones y partidas.
    private IQueryable<Torneo> TorneoCompleto => db.Torneos
        .Include(t => t.Inscripciones).ThenInclude(i => i.Equipo)
        .Include(t => t.Partidas)
        .AsSplitQuery();

    public Task<Torneo?> ObtenerAsync(int id, CancellationToken ct) =>
        TorneoCompleto.FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<Torneo?> ObtenerPorPartidaAsync(int partidaId, CancellationToken ct) =>
        TorneoCompleto.FirstOrDefaultAsync(t => t.Partidas.Any(p => p.Id == partidaId), ct);

    public void Agregar(Torneo torneo) => db.Torneos.Add(torneo);
}

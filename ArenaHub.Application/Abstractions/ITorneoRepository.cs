using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Abstractions;

public interface ITorneoRepository
{
    Task<Torneo?> ObtenerAsync(int id, CancellationToken ct);
    Task<Torneo?> ObtenerPorPartidaAsync(int partidaId, CancellationToken ct);
    void Agregar(Torneo torneo);
}

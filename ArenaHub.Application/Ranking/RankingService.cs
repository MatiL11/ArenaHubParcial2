using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;

namespace ArenaHub.Application.Ranking;

public sealed record PosicionDto(int Posicion, int EquipoId, string Equipo, int Rating, int SeriesJugadas);

public sealed class RankingService(IEquipoRepository equipos)
{
    public async Task<Result<IReadOnlyList<PosicionDto>>> ObtenerAsync(CancellationToken ct)
    {
        var porRating = await equipos.ListarPorRatingAsync(ct);

        return Result.Success<IReadOnlyList<PosicionDto>>(porRating
            .Select((e, i) => new PosicionDto(i + 1, e.Id, e.Nombre, e.Rating, e.SeriesJugadas))
            .ToList());
    }
}

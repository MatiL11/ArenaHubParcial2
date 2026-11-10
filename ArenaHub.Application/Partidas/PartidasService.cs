using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;
using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Partidas;

public sealed class PartidasService(ITorneoRepository torneos, IUnitOfWork unitOfWork)
{
    public async Task<Result<IReadOnlyList<PartidaDto>>> ListarAsync(int torneoId, CancellationToken ct)
    {
        var torneo = await torneos.ObtenerAsync(torneoId, ct);
        if (torneo is null) return TorneoErrores.NoEncontrado(torneoId);

        return Result.Success<IReadOnlyList<PartidaDto>>(
            torneo.Partidas.OrderBy(p => p.Ronda).ThenBy(p => p.Numero).Select(PartidaDto.Desde).ToList());
    }

    // Resolver la partida actualiza el Elo de los dos equipos y, si cierra la ronda, genera la siguiente.
    public async Task<Result<PartidaDto>> RegistrarResultadoAsync(int partidaId, RegistrarResultadoRequest request,
        CancellationToken ct)
    {
        var torneo = await torneos.ObtenerPorPartidaAsync(partidaId, ct);
        if (torneo is null) return PartidaErrores.NoEncontrada(partidaId);

        var partida = torneo.ResolverPartida(partidaId, request.GanadorId);
        if (partida.IsFailure) return partida.Error!;

        await unitOfWork.SaveChangesAsync(ct);
        return PartidaDto.Desde(partida.Value);
    }
}

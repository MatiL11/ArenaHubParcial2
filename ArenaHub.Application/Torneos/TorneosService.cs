using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;
using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Torneos;

public sealed class TorneosService(ITorneoRepository torneos, IUnitOfWork unitOfWork)
{
    public async Task<Result<TorneoDto>> CrearAsync(CrearTorneoRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre)) return TorneoErrores.NombreRequerido;

        var torneo = Torneo.Crear(request.Nombre.Trim(), request.Cupo);
        if (torneo.IsFailure) return torneo.Error!;

        torneos.Agregar(torneo.Value);
        await unitOfWork.SaveChangesAsync(ct);
        return TorneoDto.Desde(torneo.Value);
    }

    public async Task<Result<TorneoDto>> ObtenerAsync(int id, CancellationToken ct) =>
        await torneos.ObtenerAsync(id, ct) is { } torneo
            ? TorneoDto.Desde(torneo)
            : TorneoErrores.NoEncontrado(id);

    public async Task<Result<TorneoDto>> TransicionarAsync(int id, TransicionRequest request, CancellationToken ct)
    {
        var torneo = await torneos.ObtenerAsync(id, ct);
        if (torneo is null) return TorneoErrores.NoEncontrado(id);

        var resultado = torneo.PasarA(request.Destino);
        if (resultado.IsFailure) return resultado.Error!;

        await unitOfWork.SaveChangesAsync(ct);
        return TorneoDto.Desde(torneo);
    }
}

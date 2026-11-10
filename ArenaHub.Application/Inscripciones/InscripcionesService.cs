using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;
using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Inscripciones;

public sealed class InscripcionesService(ITorneoRepository torneos, IEquipoRepository equipos, IUnitOfWork unitOfWork)
{
    public async Task<Result<InscripcionDto>> InscribirAsync(int torneoId, InscribirEquipoRequest request,
        CancellationToken ct)
    {
        var torneo = await torneos.ObtenerAsync(torneoId, ct);
        if (torneo is null) return TorneoErrores.NoEncontrado(torneoId);

        var equipo = await equipos.ObtenerAsync(request.EquipoId, ct);
        if (equipo is null) return EquipoErrores.NoEncontrado(request.EquipoId);

        var inscripcion = torneo.Inscribir(equipo);
        if (inscripcion.IsFailure) return inscripcion.Error!;

        await unitOfWork.SaveChangesAsync(ct);
        return InscripcionDto.Desde(inscripcion.Value);
    }

    public async Task<Result<IReadOnlyList<InscripcionDto>>> ListarAsync(int torneoId, CancellationToken ct)
    {
        var torneo = await torneos.ObtenerAsync(torneoId, ct);
        if (torneo is null) return TorneoErrores.NoEncontrado(torneoId);

        return Result.Success<IReadOnlyList<InscripcionDto>>(
            torneo.Inscripciones.OrderBy(i => i.FechaInscripcion).Select(InscripcionDto.Desde).ToList());
    }
}

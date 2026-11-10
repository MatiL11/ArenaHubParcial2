using ArenaHub.Application.Partidas;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
public sealed class PartidasController(PartidasService service) : ControllerBase
{
    [HttpGet("api/torneos/{torneoId:int}/partidas")]
    [ProducesResponseType<IReadOnlyList<PartidaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Listar(int torneoId, CancellationToken ct) =>
        (await service.ListarAsync(torneoId, ct)).ToActionResult(Ok);

    [HttpPost("api/partidas/{id:int}/resultado")]
    [ProducesResponseType<PartidaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegistrarResultado(int id, RegistrarResultadoRequest request,
        CancellationToken ct) =>
        (await service.RegistrarResultadoAsync(id, request, ct)).ToActionResult(Ok);
}

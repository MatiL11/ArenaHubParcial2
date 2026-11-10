using ArenaHub.Application.Torneos;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
[Route("api/torneos")]
public sealed class TorneosController(TorneosService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<TorneoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(CrearTorneoRequest request, CancellationToken ct) =>
        (await service.CrearAsync(request, ct))
            .ToActionResult(torneo => CreatedAtAction(nameof(Obtener), new { id = torneo.Id }, torneo));

    [HttpGet("{id:int}")]
    [ProducesResponseType<TorneoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obtener(int id, CancellationToken ct) =>
        (await service.ObtenerAsync(id, ct)).ToActionResult(Ok);

    [HttpPost("{id:int}/transiciones")]
    [ProducesResponseType<TorneoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Transicionar(int id, TransicionRequest request, CancellationToken ct) =>
        (await service.TransicionarAsync(id, request, ct)).ToActionResult(Ok);
}

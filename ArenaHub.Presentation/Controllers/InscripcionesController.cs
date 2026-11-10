using ArenaHub.Application.Inscripciones;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
[Route("api/torneos/{torneoId:int}/inscripciones")]
public sealed class InscripcionesController(InscripcionesService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<InscripcionDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Inscribir(int torneoId, InscribirEquipoRequest request, CancellationToken ct) =>
        (await service.InscribirAsync(torneoId, request, ct))
            .ToActionResult(inscripcion => CreatedAtAction(nameof(Listar), new { torneoId }, inscripcion));

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<InscripcionDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Listar(int torneoId, CancellationToken ct) =>
        (await service.ListarAsync(torneoId, ct)).ToActionResult(Ok);
}

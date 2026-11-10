using ArenaHub.Application.Equipos;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
[Route("api/equipos")]
public sealed class EquiposController(EquiposService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<EquipoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(CrearEquipoRequest request, CancellationToken ct) =>
        (await service.CrearAsync(request, ct))
            .ToActionResult(equipo => CreatedAtAction(nameof(Obtener), new { id = equipo.Id }, equipo));

    [HttpGet("{id:int}")]
    [ProducesResponseType<EquipoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obtener(int id, CancellationToken ct) =>
        (await service.ObtenerAsync(id, ct)).ToActionResult(Ok);

    [HttpPost("{id:int}/jugadores")]
    [ProducesResponseType<EquipoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AgregarJugador(int id, AgregarJugadorRequest request, CancellationToken ct) =>
        (await service.AgregarJugadorAsync(id, request, ct)).ToActionResult(Ok);
}

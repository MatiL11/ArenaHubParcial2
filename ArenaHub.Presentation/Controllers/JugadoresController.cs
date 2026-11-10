using ArenaHub.Application.Jugadores;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
[Route("api/jugadores")]
public sealed class JugadoresController(JugadoresService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<JugadorDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(CrearJugadorRequest request, CancellationToken ct) =>
        (await service.CrearAsync(request, ct))
            .ToActionResult(jugador => CreatedAtAction(nameof(Obtener), new { id = jugador.Id }, jugador));

    [HttpGet("{id:int}")]
    [ProducesResponseType<JugadorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obtener(int id, CancellationToken ct) =>
        (await service.ObtenerAsync(id, ct)).ToActionResult(Ok);
}

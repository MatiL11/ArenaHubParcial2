using ArenaHub.Application.Ranking;
using ArenaHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Controllers;

[ApiController]
[Route("api/ranking")]
public sealed class RankingController(RankingService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<PosicionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Obtener(CancellationToken ct) =>
        (await service.ObtenerAsync(ct)).ToActionResult(Ok);
}

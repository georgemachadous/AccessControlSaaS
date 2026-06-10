using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;

namespace AccessControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Gestor")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    [ProducesResponseType(typeof(DashboardStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardStatsDto>> GetStats(CancellationToken ct)
    {
        var query = new GetDashboardStatsQuery();
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("logins-por-dia")]
    [ProducesResponseType(typeof(IEnumerable<LoginPorDiaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LoginPorDiaDto>>> GetLoginsPorDia(
        [FromQuery] int dias = 30, CancellationToken ct = default)
    {
        var query = new GetLoginsPorDiaQuery(dias);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}

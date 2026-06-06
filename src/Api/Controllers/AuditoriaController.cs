
using Microsoft.AspNetCore.Mvc;
using AccessControl.Application.DTOs;

using Microsoft.AspNetCore.Authorization;

using MediatR;

using AccessControl.Application.Queries;

using AccessControl.Domain.Entities;

namespace AccessControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AuditoriaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditoriaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<LogAuditoria>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<LogAuditoria>>> GetAll(
        [FromQuery] string? entidade = null, [FromQuery] string? acao = null,
        [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var query = new GetAuditoriaQuery(entidade, acao, from, to, page, pageSize);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}


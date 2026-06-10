using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;

namespace AccessControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class NotificacoesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificacoesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<NotificacaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<NotificacaoDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? lida = null, CancellationToken ct = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var query = new GetNotificacoesByUsuarioQuery(Guid.Parse(userId), lida, page, pageSize);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("nao-lidas/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetNaoLidasCount(CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var query = new GetNotificacoesNaoLidasCountQuery(Guid.Parse(userId));
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/ler")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarcarComoLida(Guid id, CancellationToken ct)
    {
        var command = new MarcarNotificacaoLidaCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? Ok(new { message = "Notification marked as read" }) : NotFound();
    }
}

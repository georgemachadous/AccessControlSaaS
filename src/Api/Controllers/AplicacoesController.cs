using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;

namespace AccessControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AplicacoesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AplicacoesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<AplicacaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<AplicacaoDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null,
        [FromQuery] bool? ativa = null, CancellationToken ct = default)
    {
        var query = new GetAllAplicacoesQuery(page, pageSize, search, ativa);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AplicacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AplicacaoDto>> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetAplicacaoByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(AplicacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AplicacaoDto>> Create([FromBody] CriarAplicacaoDto request, CancellationToken ct)
    {
        var command = new CriarAplicacaoCommand(request);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AplicacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AplicacaoDto>> Update(Guid id, [FromBody] AtualizarAplicacaoDto request, CancellationToken ct)
    {
        if (id != request.Id) return BadRequest(new { message = "ID mismatch" });
        var command = new AtualizarAplicacaoCommand(request);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new ExcluirAplicacaoCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/usuarios")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios(Guid id, CancellationToken ct)
    {
        var query = new GetUsuariosByAplicacaoQuery(id);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}

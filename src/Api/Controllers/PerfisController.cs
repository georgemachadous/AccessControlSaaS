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
public class PerfisController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerfisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(PaginatedResult<PerfilDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<PerfilDto>>> GetAll(
        [FromQuery] Guid empresaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var query = new GetPerfisByEmpresaQuery(empresaId, page, pageSize);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfilDto>> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetPerfilByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PerfilDto>> Create([FromBody] CriarPerfilDto request, CancellationToken ct)
    {
        var command = new CriarPerfilCommand(request);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfilDto>> Update(Guid id, [FromBody] AtualizarPerfilDto request, CancellationToken ct)
    {
        if (id != request.Id) return BadRequest(new { message = "ID mismatch" });
        var command = new AtualizarPerfilCommand(request);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new ExcluirPerfilCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/funcionalidades")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(IEnumerable<PerfilFuncionalidadeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PerfilFuncionalidadeDto>>> GetFuncionalidades(Guid id, CancellationToken ct)
    {
        var query = new GetFuncionalidadesByPerfilQuery(id);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/funcionalidades")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PerfilFuncionalidadeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PerfilFuncionalidadeDto>> AtribuirFuncionalidade(Guid id, [FromBody] AtribuirFuncionalidadeDto request, CancellationToken ct)
    {
        if (id != request.PerfilId) return BadRequest(new { message = "ID mismatch" });
        var command = new AtribuirFuncionalidadeCommand(request);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/funcionalidades/{funcionalidadeId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverFuncionalidade(Guid id, Guid funcionalidadeId, CancellationToken ct)
    {
        var command = new RemoverFuncionalidadeCommand(id, funcionalidadeId);
        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }
}

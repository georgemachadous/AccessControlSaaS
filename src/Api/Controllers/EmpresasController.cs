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
public class EmpresasController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmpresasController> _logger;

    public EmpresasController(IMediator mediator, ILogger<EmpresasController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(PaginatedResult<EmpresaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<EmpresaDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] bool? ativa = null,
        CancellationToken ct = default)
    {
        var query = new GetAllEmpresasQuery(page, pageSize, search, ativa);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpresaDto>> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetEmpresaByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpresaDto>> Create([FromBody] CriarEmpresaDto request, CancellationToken ct)
    {
        var command = new CriarEmpresaCommand(request);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpresaDto>> Update(Guid id, [FromBody] AtualizarEmpresaDto request, CancellationToken ct)
    {
        if (id != request.Id) return BadRequest(new { message = "ID mismatch" });
        var command = new AtualizarEmpresaCommand(request);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new ExcluirEmpresaCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/stats")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(DashboardStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardStatsDto>> GetStats(Guid id, CancellationToken ct)
    {
        var query = new GetEmpresaStatsQuery(id);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/logo")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadLogo(Guid id, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest(new { message = "File is empty" });
        if (file.Length > 5 * 1024 * 1024) return BadRequest(new { message = "File too large (max 5MB)" });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/svg+xml" };
        if (!allowedTypes.Contains(file.ContentType)) return BadRequest(new { message = "Invalid file type" });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var fileData = ms.ToArray();

        var command = new UploadLogoEmpresaCommand(id, fileData, file.FileName, file.ContentType);
        var result = await _mediator.Send(command, ct);
        return Ok(new { logoUrl = result });
    }
}

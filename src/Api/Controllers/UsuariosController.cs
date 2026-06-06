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
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IMediator mediator, ILogger<UsuariosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(PaginatedResult<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<UsuarioDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null,
        [FromQuery] Guid? empresaId = null, [FromQuery] Guid? filialId = null, [FromQuery] bool? ativo = null,
        CancellationToken ct = default)
    {
        var query = new GetAllUsuariosQuery(page, pageSize, search, empresaId, filialId, ativo);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Gestor,Operador")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetUsuarioByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] CriarUsuarioDto request, CancellationToken ct)
    {
        var command = new CriarUsuarioCommand(request);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Update(Guid id, [FromBody] AtualizarUsuarioDto request, CancellationToken ct)
    {
        if (id != request.Id) return BadRequest(new { message = "ID mismatch" });
        var command = new AtualizarUsuarioCommand(request);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new ExcluirUsuarioCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/ativar")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken ct)
    {
        var command = new AtivarUsuarioCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? Ok(new { message = "User activated" }) : NotFound();
    }

    [HttpPost("{id:guid}/desativar")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
    {
        var command = new DesativarUsuarioCommand(id);
        var result = await _mediator.Send(command, ct);
        return result ? Ok(new { message = "User deactivated" }) : NotFound();
    }

    [HttpPost("{id:guid}/avatar")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadAvatar(Guid id, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest(new { message = "File is empty" });
        if (file.Length > 2 * 1024 * 1024) return BadRequest(new { message = "File too large (max 2MB)" });

        var allowedTypes = new[] { "image/jpeg", "image/png" };
        if (!allowedTypes.Contains(file.ContentType)) return BadRequest(new { message = "Invalid file type" });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var fileData = ms.ToArray();

        var command = new UploadAvatarUsuarioCommand(id, fileData, file.FileName, file.ContentType);
        var result = await _mediator.Send(command, ct);
        return Ok(new { avatarUrl = result });
    }

    [HttpGet("{id:guid}/perfis")]
    [Authorize(Roles = "Admin,Gestor")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioPerfilAplicacaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioPerfilAplicacaoDto>>> GetPerfis(Guid id, CancellationToken ct)
    {
        // Implementation would query the user's profiles
        return Ok(new List<UsuarioPerfilAplicacaoDto>());
    }

    [HttpPost("{id:guid}/perfis")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UsuarioPerfilAplicacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioPerfilAplicacaoDto>> AtribuirPerfil(Guid id, [FromBody] AtribuirPerfilDto request, CancellationToken ct)
    {
        if (id != request.UsuarioId) return BadRequest(new { message = "ID mismatch" });
        var command = new AtribuirPerfilCommand(request);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetPerfis), new { id }, result);
    }
}

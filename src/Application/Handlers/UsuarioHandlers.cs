using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Entities;
using AccessControl.Domain.Interfaces;
using AccessControl.Infrastructure.Persistence;

namespace AccessControl.Application.Handlers;

public class UsuarioHandlers :
    IRequestHandler<CriarUsuarioCommand, UsuarioDto>,
    IRequestHandler<AtualizarUsuarioCommand, UsuarioDto>,
    IRequestHandler<ExcluirUsuarioCommand, bool>,
    IRequestHandler<AtivarUsuarioCommand, bool>,
    IRequestHandler<DesativarUsuarioCommand, bool>,
    IRequestHandler<GetUsuarioByIdQuery, UsuarioDto?>,
    IRequestHandler<GetAllUsuariosQuery, PaginatedResult<UsuarioDto>>
{
    private readonly AccessControlDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    private readonly ITenantContext _tenantContext;

    public UsuarioHandlers(AccessControlDbContext context, IMapper mapper, IAuditService auditService, ITenantContext tenantContext)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
        _tenantContext = tenantContext;
    }

    public async Task<UsuarioDto> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = _mapper.Map<Usuario>(request.Dto);
        usuario.TenantId = _tenantContext.CurrentTenantId;
        usuario.CreatedBy = _tenantContext.CurrentUserId;

        if (!request.Dto.IsSsoUser && !string.IsNullOrEmpty(request.Dto.Senha))
        {
            // Hash password with BCrypt
        }

        if (string.IsNullOrEmpty(usuario.Idioma) && usuario.FilialId.HasValue)
        {
            var filial = await _context.Filiais.FindAsync(new object[] { usuario.FilialId.Value }, cancellationToken);
            if (filial is not null && !string.IsNullOrEmpty(filial.Idioma))
                usuario.Idioma = filial.Idioma;
        }
        if (string.IsNullOrEmpty(usuario.Idioma))
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.TenantId == usuario.TenantId, cancellationToken);
            usuario.Idioma = empresa?.IdiomaPadrao ?? "pt";
        }

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("CREATE", "Usuario", usuario.Id.ToString(), null, null, $"Usuario {usuario.Nome} criado");
        return _mapper.Map<UsuarioDto>(usuario);
    }

    public async Task<UsuarioDto> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (usuario is null) throw new KeyNotFoundException($"Usuario {request.Dto.Id} not found");

        var valorAnterior = System.Text.Json.JsonSerializer.Serialize(usuario);
        _mapper.Map(request.Dto, usuario);
        usuario.UpdatedBy = _tenantContext.CurrentUserId;

        await _context.SaveChangesAsync(cancellationToken);

        var valorNovo = System.Text.Json.JsonSerializer.Serialize(usuario);
        await _auditService.LogAsync("UPDATE", "Usuario", usuario.Id.ToString(), valorAnterior, valorNovo, $"Usuario {usuario.Nome} atualizado");
        return _mapper.Map<UsuarioDto>(usuario);
    }

    public async Task<bool> Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
        if (usuario is null) return false;

        usuario.IsDeleted = true;
        usuario.DeletedAt = DateTime.UtcNow;
        usuario.Ativo = false;
        usuario.UpdatedBy = _tenantContext.CurrentUserId;

        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("DELETE", "Usuario", usuario.Id.ToString(), null, null, $"Usuario {usuario.Nome} excluido");
        return true;
    }

    public async Task<bool> Handle(AtivarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
        if (usuario is null) return false;

        usuario.Ativo = true;
        usuario.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("ACTIVATE", "Usuario", usuario.Id.ToString(), null, null, $"Usuario {usuario.Nome} ativado");
        return true;
    }

    public async Task<bool> Handle(DesativarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
        if (usuario is null) return false;

        usuario.Ativo = false;
        usuario.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("DEACTIVATE", "Usuario", usuario.Id.ToString(), null, null, $"Usuario {usuario.Nome} desativado");
        return true;
    }

    public async Task<UsuarioDto?> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
        return usuario is not null ? _mapper.Map<UsuarioDto>(usuario) : null;
    }

    public async Task<PaginatedResult<UsuarioDto>> Handle(GetAllUsuariosQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(u => u.Nome.Contains(request.Search) || u.Email.Contains(request.Search) || (u.Cpf != null && u.Cpf.Contains(request.Search)));

        if (request.EmpresaId.HasValue)
            query = query.Where(u => u.TenantId == request.EmpresaId.Value);

        if (request.FilialId.HasValue)
            query = query.Where(u => u.FilialId == request.FilialId);

        if (request.Ativo.HasValue)
            query = query.Where(u => u.Ativo == request.Ativo.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<UsuarioDto>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PaginatedResult<UsuarioDto>(dtos, totalCount, request.Page, request.PageSize, totalPages);
    }
}

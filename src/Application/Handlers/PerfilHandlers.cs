using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Interfaces;
using AccessControl.Infrastructure.Persistence;

namespace AccessControl.Application.Handlers;

public class PerfilHandlers :
    IRequestHandler<CriarPerfilCommand, PerfilDto>,
    IRequestHandler<AtualizarPerfilCommand, PerfilDto>,
    IRequestHandler<ExcluirPerfilCommand, bool>,
    IRequestHandler<GetPerfilByIdQuery, PerfilDto?>,
    IRequestHandler<GetPerfisByEmpresaQuery, PaginatedResult<PerfilDto>>,
    IRequestHandler<AtribuirFuncionalidadeCommand, PerfilFuncionalidadeDto>,
    IRequestHandler<RemoverFuncionalidadeCommand, bool>
{
    private readonly AccessControlDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    private readonly ITenantContext _tenantContext;

    public PerfilHandlers(AccessControlDbContext context, IMapper mapper, IAuditService auditService, ITenantContext tenantContext)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
        _tenantContext = tenantContext;
    }

    public async Task<PerfilDto> Handle(CriarPerfilCommand request, CancellationToken cancellationToken)
    {
        var perfil = _mapper.Map<Domain.Entities.Perfil>(request.Dto);
        perfil.TenantId = _tenantContext.CurrentTenantId;
        perfil.CreatedBy = _tenantContext.CurrentUserId;

        _context.Perfis.Add(perfil);
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("CREATE", "Perfil", perfil.Id.ToString(), null, null, $"Perfil {perfil.Nome} criado");
        return _mapper.Map<PerfilDto>(perfil);
    }

    public async Task<PerfilDto> Handle(AtualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var perfil = await _context.Perfis.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (perfil is null) throw new KeyNotFoundException($"Perfil {request.Dto.Id} not found");

        _mapper.Map(request.Dto, perfil);
        perfil.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("UPDATE", "Perfil", perfil.Id.ToString(), null, null, $"Perfil {perfil.Nome} atualizado");
        return _mapper.Map<PerfilDto>(perfil);
    }

    public async Task<bool> Handle(ExcluirPerfilCommand request, CancellationToken cancellationToken)
    {
        var perfil = await _context.Perfis.FindAsync(new object[] { request.Id }, cancellationToken);
        if (perfil is null) return false;

        perfil.IsDeleted = true;
        perfil.DeletedAt = DateTime.UtcNow;
        perfil.Ativo = false;
        perfil.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("DELETE", "Perfil", perfil.Id.ToString(), null, null, $"Perfil {perfil.Nome} excluido");
        return true;
    }

    public async Task<PerfilDto?> Handle(GetPerfilByIdQuery request, CancellationToken cancellationToken)
    {
        var perfil = await _context.Perfis.FindAsync(new object[] { request.Id }, cancellationToken);
        return perfil is not null ? _mapper.Map<PerfilDto>(perfil) : null;
    }

    public async Task<PaginatedResult<PerfilDto>> Handle(GetPerfisByEmpresaQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Perfis.Where(p => p.EmpresaId == request.EmpresaId && !p.IsDeleted);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.Nome)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PerfilDto>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        return new PaginatedResult<PerfilDto>(dtos, totalCount, request.Page, request.PageSize, totalPages);
    }

    public async Task<PerfilFuncionalidadeDto> Handle(AtribuirFuncionalidadeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.PerfilFuncionalidades
            .FirstOrDefaultAsync(pf => pf.PerfilId == request.Dto.PerfilId && pf.FuncionalidadeId == request.Dto.FuncionalidadeId, cancellationToken);

        if (existing != null)
        {
            existing.PermiteCriar = request.Dto.PermiteCriar;
            existing.PermiteLer = request.Dto.PermiteLer;
            existing.PermiteAtualizar = request.Dto.PermiteAtualizar;
            existing.PermiteDeletar = request.Dto.PermiteDeletar;
            existing.PermiteExecutar = request.Dto.PermiteExecutar;
            existing.UpdatedBy = _tenantContext.CurrentUserId;
        }
        else
        {
            var pf = new Domain.Entities.PerfilFuncionalidade
            {
                PerfilId = request.Dto.PerfilId,
                FuncionalidadeId = request.Dto.FuncionalidadeId,
                PermiteCriar = request.Dto.PermiteCriar,
                PermiteLer = request.Dto.PermiteLer,
                PermiteAtualizar = request.Dto.PermiteAtualizar,
                PermiteDeletar = request.Dto.PermiteDeletar,
                PermiteExecutar = request.Dto.PermiteExecutar,
                TenantId = _tenantContext.CurrentTenantId,
                CreatedBy = _tenantContext.CurrentUserId
            };
            _context.PerfilFuncionalidades.Add(pf);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("ASSIGN", "PerfilFuncionalidade", $"{request.Dto.PerfilId}-{request.Dto.FuncionalidadeId}", null, null, "Funcionalidade atribuida ao perfil");

        return new PerfilFuncionalidadeDto(request.Dto.PerfilId, request.Dto.FuncionalidadeId, "", "", request.Dto.PermiteCriar, request.Dto.PermiteLer, request.Dto.PermiteAtualizar, request.Dto.PermiteDeletar, request.Dto.PermiteExecutar);
    }

    public async Task<bool> Handle(RemoverFuncionalidadeCommand request, CancellationToken cancellationToken)
    {
        var pf = await _context.PerfilFuncionalidades
            .FirstOrDefaultAsync(pf => pf.PerfilId == request.PerfilId && pf.FuncionalidadeId == request.FuncionalidadeId, cancellationToken);
        if (pf is null) return false;

        _context.PerfilFuncionalidades.Remove(pf);
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("REMOVE", "PerfilFuncionalidade", $"{request.PerfilId}-{request.FuncionalidadeId}", null, null, "Funcionalidade removida do perfil");
        return true;
    }
}

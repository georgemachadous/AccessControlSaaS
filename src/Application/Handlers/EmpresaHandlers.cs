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

public class EmpresaHandlers :
    IRequestHandler<CriarEmpresaCommand, EmpresaDto>,
    IRequestHandler<AtualizarEmpresaCommand, EmpresaDto>,
    IRequestHandler<ExcluirEmpresaCommand, bool>,
    IRequestHandler<GetEmpresaByIdQuery, EmpresaDto?>,
    IRequestHandler<GetAllEmpresasQuery, PaginatedResult<EmpresaDto>>
{
    private readonly AccessControlDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    private readonly ITenantContext _tenantContext;

    public EmpresaHandlers(AccessControlDbContext context, IMapper mapper, IAuditService auditService, ITenantContext tenantContext)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
        _tenantContext = tenantContext;
    }

    public async Task<EmpresaDto> Handle(CriarEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = _mapper.Map<Empresa>(request.Dto);
        empresa.TenantId = Guid.NewGuid();
        empresa.CreatedBy = _tenantContext.CurrentUserId;

        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("CREATE", "Empresa", empresa.Id.ToString(), null, null, $"Empresa {empresa.Nome} criada");
        return _mapper.Map<EmpresaDto>(empresa);
    }

    public async Task<EmpresaDto> Handle(AtualizarEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _context.Empresas.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (empresa is null) throw new KeyNotFoundException($"Empresa {request.Dto.Id} not found");

        var valorAnterior = System.Text.Json.JsonSerializer.Serialize(empresa);
        _mapper.Map(request.Dto, empresa);
        empresa.UpdatedBy = _tenantContext.CurrentUserId;

        await _context.SaveChangesAsync(cancellationToken);

        var valorNovo = System.Text.Json.JsonSerializer.Serialize(empresa);
        await _auditService.LogAsync("UPDATE", "Empresa", empresa.Id.ToString(), valorAnterior, valorNovo, $"Empresa {empresa.Nome} atualizada");
        return _mapper.Map<EmpresaDto>(empresa);
    }

    public async Task<bool> Handle(ExcluirEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _context.Empresas.FindAsync(new object[] { request.Id }, cancellationToken);
        if (empresa is null) return false;

        empresa.IsDeleted = true;
        empresa.DeletedAt = DateTime.UtcNow;
        empresa.UpdatedBy = _tenantContext.CurrentUserId;

        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync("DELETE", "Empresa", empresa.Id.ToString(), null, null, $"Empresa {empresa.Nome} excluida");
        return true;
    }

    public async Task<EmpresaDto?> Handle(GetEmpresaByIdQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _context.Empresas.FindAsync(new object[] { request.Id }, cancellationToken);
        return empresa is not null ? _mapper.Map<EmpresaDto>(empresa) : null;
    }

    public async Task<PaginatedResult<EmpresaDto>> Handle(GetAllEmpresasQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Empresas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(e => e.Nome.Contains(request.Search) || e.Cnpj.Contains(request.Search) || e.Email.Contains(request.Search));

        if (request.Ativa.HasValue)
            query = query.Where(e => e.Ativa == request.Ativa.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<EmpresaDto>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PaginatedResult<EmpresaDto>(dtos, totalCount, request.Page, request.PageSize, totalPages);
    }
}

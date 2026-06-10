using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Interfaces;
using AccessControl.Infrastructure.Persistence;

namespace AccessControl.Application.Handlers;

public class AplicacaoHandlers :
    IRequestHandler<CriarAplicacaoCommand, AplicacaoDto>,
    IRequestHandler<AtualizarAplicacaoCommand, AplicacaoDto>,
    IRequestHandler<ExcluirAplicacaoCommand, bool>,
    IRequestHandler<GetAplicacaoByIdQuery, AplicacaoDto?>,
    IRequestHandler<GetAllAplicacoesQuery, PaginatedResult<AplicacaoDto>>
{
    private readonly AccessControlDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    private readonly ITenantContext _tenantContext;

    public AplicacaoHandlers(AccessControlDbContext context, IMapper mapper, IAuditService auditService, ITenantContext tenantContext)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
        _tenantContext = tenantContext;
    }

    public async Task<AplicacaoDto> Handle(CriarAplicacaoCommand request, CancellationToken cancellationToken)
    {
        var app = _mapper.Map<Domain.Entities.Aplicacao>(request.Dto);
        app.TenantId = _tenantContext.CurrentTenantId;
        app.CreatedBy = _tenantContext.CurrentUserId;

        _context.Aplicacoes.Add(app);
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("CREATE", "Aplicacao", app.Id.ToString(), null, null, $"Aplicacao {app.Nome} criada");
        return _mapper.Map<AplicacaoDto>(app);
    }

    public async Task<AplicacaoDto> Handle(AtualizarAplicacaoCommand request, CancellationToken cancellationToken)
    {
        var app = await _context.Aplicacoes.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (app is null) throw new KeyNotFoundException($"Aplicacao {request.Dto.Id} not found");

        _mapper.Map(request.Dto, app);
        app.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("UPDATE", "Aplicacao", app.Id.ToString(), null, null, $"Aplicacao {app.Nome} atualizada");
        return _mapper.Map<AplicacaoDto>(app);
    }

    public async Task<bool> Handle(ExcluirAplicacaoCommand request, CancellationToken cancellationToken)
    {
        var app = await _context.Aplicacoes.FindAsync(new object[] { request.Id }, cancellationToken);
        if (app is null) return false;

        app.IsDeleted = true;
        app.DeletedAt = DateTime.UtcNow;
        app.Ativa = false;
        app.UpdatedBy = _tenantContext.CurrentUserId;
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync("DELETE", "Aplicacao", app.Id.ToString(), null, null, $"Aplicacao {app.Nome} excluida");
        return true;
    }

    public async Task<AplicacaoDto?> Handle(GetAplicacaoByIdQuery request, CancellationToken cancellationToken)
    {
        var app = await _context.Aplicacoes.FindAsync(new object[] { request.Id }, cancellationToken);
        return app is not null ? _mapper.Map<AplicacaoDto>(app) : null;
    }

    public async Task<PaginatedResult<AplicacaoDto>> Handle(GetAllAplicacoesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Aplicacoes.Where(a => !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(a => a.Nome.Contains(request.Search) || a.Codigo.Contains(request.Search));

        if (request.Ativa.HasValue)
            query = query.Where(a => a.Ativa == request.Ativa.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(a => a.Nome)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<AplicacaoDto>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        return new PaginatedResult<AplicacaoDto>(dtos, totalCount, request.Page, request.PageSize, totalPages);
    }
}

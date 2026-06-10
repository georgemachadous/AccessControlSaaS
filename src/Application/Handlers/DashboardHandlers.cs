using Microsoft.EntityFrameworkCore;
using MediatR;
using AccessControl.Application.Queries;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Interfaces;
using AccessControl.Infrastructure.Persistence;

namespace AccessControl.Application.Handlers;

public class DashboardHandlers :
    IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>,
    IRequestHandler<GetLoginsPorDiaQuery, IEnumerable<LoginPorDiaDto>>
{
    private readonly AccessControlDbContext _context;
    private readonly ITenantContext _tenantContext;

    public DashboardHandlers(AccessControlDbContext context, ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var today = DateTime.UtcNow.Date;

        var totalEmpresas = await _context.Empresas.CountAsync(e => e.Ativa && !e.IsDeleted, cancellationToken);
        var totalUsuarios = await _context.Usuarios.CountAsync(u => u.Ativo && !u.IsDeleted && (tenantId == Guid.Empty || u.TenantId == tenantId), cancellationToken);
        var totalAplicacoes = await _context.Aplicacoes.CountAsync(a => a.Ativa && !a.IsDeleted, cancellationToken);
        var totalPerfis = await _context.Perfis.CountAsync(p => p.Ativo && !p.IsDeleted && (tenantId == Guid.Empty || p.TenantId == tenantId), cancellationToken);
        var loginsHoje = await _context.SessoesUsuarios.CountAsync(s => s.DataCriacao.Date == today && s.IsValida, cancellationToken);
        var usuariosAtivos = await _context.Usuarios.CountAsync(u => u.UltimoAcesso > today.AddDays(-30) && u.Ativo && !u.IsDeleted, cancellationToken);
        var sessoesAtivas = await _context.SessoesUsuarios.CountAsync(s => s.IsValida && s.DataExpiracao > DateTime.UtcNow, cancellationToken);

        var loginsPorDia = await _context.SessoesUsuarios
            .Where(s => s.DataCriacao > today.AddDays(-30) && s.IsValida)
            .GroupBy(s => s.DataCriacao.Date)
            .Select(g => new LoginPorDiaDto(g.Key, g.Count()))
            .OrderBy(x => x.Data)
            .ToListAsync(cancellationToken);

        return new DashboardStatsDto(totalEmpresas, totalUsuarios, totalAplicacoes, totalPerfis, loginsHoje, usuariosAtivos, sessoesAtivas, loginsPorDia);
    }

    public async Task<IEnumerable<LoginPorDiaDto>> Handle(GetLoginsPorDiaQuery request, CancellationToken cancellationToken)
    {
        var fromDate = DateTime.UtcNow.Date.AddDays(-request.Dias);
        return await _context.SessoesUsuarios
            .Where(s => s.DataCriacao > fromDate && s.IsValida)
            .GroupBy(s => s.DataCriacao.Date)
            .Select(g => new LoginPorDiaDto(g.Key, g.Count()))
            .OrderBy(x => x.Data)
            .ToListAsync(cancellationToken);
    }
}

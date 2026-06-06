namespace AccessControl.Domain.Interfaces;

public interface IAuditService
{
    Task LogAsync(string acao, string entidade, string entidadeId, string? valorAnterior, string? valorNovo, string? observacao = null);
    Task<IEnumerable<Entities.LogAuditoria>> GetByEntityAsync(string entidade, string entidadeId, CancellationToken ct = default);
    Task<IEnumerable<Entities.LogAuditoria>> GetByTenantAsync(Guid tenantId, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
}

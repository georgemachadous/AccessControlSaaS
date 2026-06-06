namespace AccessControl.Domain.Entities;

public class LogAuditoria : BaseEntity
{
    public string Acao { get; set; } = string.Empty;
    public string Entidade { get; set; } = string.Empty;
    public string EntidadeId { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNovo { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string? Endpoint { get; set; }
    public string? MetodoHttp { get; set; }
    public int? StatusCode { get; set; }
    public string? CorrelationId { get; set; }
    public string? Observacao { get; set; }
}

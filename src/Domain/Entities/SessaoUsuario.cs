namespace AccessControl.Domain.Entities;

public class SessaoUsuario : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public string TokenJti { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataExpiracao { get; set; }
    public DateTime? DataUltimaAtividade { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
    public bool IsValida { get; set; } = true;
    public DateTime? DataInvalidacao { get; set; }
    public string? MotivoInvalidacao { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

namespace AccessControl.Domain.Entities;

public class UsuarioPerfilAplicacao : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Guid PerfilId { get; set; }
    public Guid AplicacaoId { get; set; }
    public bool IsPrincipal { get; set; }
    public DateTime? DataAtribuicao { get; set; } = DateTime.UtcNow;
    public DateTime? DataExpiracao { get; set; }
    public string AtribuidoPor { get; set; } = string.Empty;
    public Usuario Usuario { get; set; } = null!;
    public Perfil Perfil { get; set; } = null!;
    public Aplicacao Aplicacao { get; set; } = null!;
}

namespace AccessControl.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // Password hash for local authentication
    public string SenhaHash { get; set; } = string.Empty;
    // Empresa/Tenant association (nullable for SSO-only or system users)
    public Guid? EmpresaId { get; set; }
    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
    public string Idioma { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool Ativo { get; set; } = true;
    public bool EmailConfirmado { get; set; }
    public bool MfaHabilitado { get; set; }
    public string? MfaSecretKey { get; set; }
    public DateTime? UltimoAcesso { get; set; }
    public int TentativasFalhasLogin { get; set; }
    public DateTime? BloqueadoAte { get; set; }
    public bool IsSsoUser { get; set; }
    public string? SsoProvider { get; set; }
    public string? SsoExternalId { get; set; }
    public Guid? FilialId { get; set; }
    public Filial? Filial { get; set; }
    public ICollection<UsuarioPerfilAplicacao> UsuarioPerfilAplicacoes { get; set; } = new List<UsuarioPerfilAplicacao>();
    public ICollection<SessaoUsuario> Sessoes { get; set; } = new List<SessaoUsuario>();
    public ICollection<Notificacao> Notificacoes { get; set; } = new List<Notificacao>();
}

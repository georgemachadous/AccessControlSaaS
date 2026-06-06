namespace AccessControl.Domain.Entities;

public class Aplicacao : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public string? Cor { get; set; }
    public bool Ativa { get; set; } = true;
    public bool IsPublica { get; set; }
    public string? SsoClientId { get; set; }
    public string? SsoClientSecret { get; set; }
    public string? SsoAuthority { get; set; }
    public string? SsoScopes { get; set; }
    public ICollection<EmpresaAplicacao> EmpresaAplicacoes { get; set; } = new List<EmpresaAplicacao>();
    public ICollection<Funcionalidade> Funcionalidades { get; set; } = new List<Funcionalidade>();
}

namespace AccessControl.Domain.Entities;

public class Empresa : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string IdiomaPadrao { get; set; } = "pt";
    public bool Ativa { get; set; } = true;
    public string Cep { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = "BR";
    public string ConfiguracoesJSON { get; set; } = "{}";
    public ICollection<Filial> Filiais { get; set; } = new List<Filial>();
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<EmpresaAplicacao> EmpresaAplicacoes { get; set; } = new List<EmpresaAplicacao>();
    public ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
}

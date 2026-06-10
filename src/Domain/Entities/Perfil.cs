namespace AccessControl.Domain.Entities;

public class Perfil : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool IsPadrao { get; set; }
    public bool Ativo { get; set; } = true;
    public Guid EmpresaId { get; set; }
    public Empresa Empresa { get; set; } = null!;
    public ICollection<PerfilFuncionalidade> PerfilFuncionalidades { get; set; } = new List<PerfilFuncionalidade>();
    public ICollection<UsuarioPerfilAplicacao> UsuarioPerfilAplicacoes { get; set; } = new List<UsuarioPerfilAplicacao>();
}

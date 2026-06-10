namespace AccessControl.Domain.Entities;

public class Funcionalidade : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public string? Rota { get; set; }
    public int Ordem { get; set; }
    public bool Ativa { get; set; } = true;
    public bool IsPadrao { get; set; }
    public Guid? AplicacaoId { get; set; }
    public Aplicacao? Aplicacao { get; set; }
    public ICollection<PerfilFuncionalidade> PerfilFuncionalidades { get; set; } = new List<PerfilFuncionalidade>();
}

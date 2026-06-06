namespace AccessControl.Domain.Entities;

public class EmpresaAplicacao : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public Guid AplicacaoId { get; set; }
    public bool Ativa { get; set; } = true;
    public DateTime? DataAtivacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataExpiracao { get; set; }
    public int LimiteUsuarios { get; set; } = 0;
    public string ConfiguracoesJSON { get; set; } = "{}";
    public Empresa Empresa { get; set; } = null!;
    public Aplicacao Aplicacao { get; set; } = null!;
}

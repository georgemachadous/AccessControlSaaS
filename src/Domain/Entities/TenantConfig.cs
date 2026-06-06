namespace AccessControl.Domain.Entities;

public class TenantConfig : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Tipo { get; set; } = "string";
    public string Descricao { get; set; } = string.Empty;
    public bool IsSobrescreverGlobal { get; set; } = true;
    public Empresa Empresa { get; set; } = null!;
}

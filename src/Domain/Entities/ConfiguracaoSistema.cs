namespace AccessControl.Domain.Entities;

public class ConfiguracaoSistema : BaseEntity
{
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Tipo { get; set; } = "string";
    public string Descricao { get; set; } = string.Empty;
    public bool IsPublico { get; set; }
    public string? Categoria { get; set; }
}

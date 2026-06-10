namespace AccessControl.Domain.Entities;

public class Filial : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;
    public string Cep { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public Empresa Empresa { get; set; } = null!;
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

namespace AccessControl.Domain.Entities;

public class Notificacao : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Tipo { get; set; } = "info";
    public string? Link { get; set; }
    public bool Lida { get; set; }
    public DateTime? DataLeitura { get; set; }
    public string? Icone { get; set; }
    public string? Categoria { get; set; }
    public bool EnviarPush { get; set; }
    public bool EnviarEmail { get; set; }
    public DateTime? DataEnvioEmail { get; set; }
    public DateTime? DataEnvioPush { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

namespace AccessControl.Domain.Events;

public abstract class DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public class UsuarioCriadoEvent : DomainEvent
{
    public Guid UsuarioId { get; init; }
    public string Email { get; init; } = string.Empty;
    public Guid TenantId { get; init; }
}

public class UsuarioLogadoEvent : DomainEvent
{
    public Guid UsuarioId { get; init; }
    public string IpAddress { get; init; } = string.Empty;
    public string UserAgent { get; init; } = string.Empty;
}

public class PerfilAtribuidoEvent : DomainEvent
{
    public Guid UsuarioId { get; init; }
    public Guid PerfilId { get; init; }
    public Guid AplicacaoId { get; init; }
    public string AtribuidoPor { get; init; } = string.Empty;
}

public class TentativaLoginFalhaEvent : DomainEvent
{
    public string Email { get; init; } = string.Empty;
    public string IpAddress { get; init; } = string.Empty;
    public int TentativaNumero { get; init; }
}

public class UsuarioBloqueadoEvent : DomainEvent
{
    public Guid UsuarioId { get; init; }
    public DateTime BloqueadoAte { get; init; }
    public string Motivo { get; init; } = string.Empty;
}

public class EmpresaCriadaEvent : DomainEvent
{
    public Guid EmpresaId { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Cnpj { get; init; } = string.Empty;
}

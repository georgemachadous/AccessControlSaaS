namespace AccessControl.Domain.Interfaces;

public interface INotificationService
{
    Task SendAsync(Guid userId, string titulo, string mensagem, string tipo = "info", string? link = null, bool push = false, bool email = false, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid notificacaoId, CancellationToken ct = default);
    Task<IEnumerable<Entities.Notificacao>> GetUnreadAsync(Guid userId, CancellationToken ct = default);
}

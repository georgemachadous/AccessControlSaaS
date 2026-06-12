namespace AccessControl.Domain.Interfaces;

public interface IAuthService
{
    Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> AuthenticateAsync(string email, string password, string ipAddress, string userAgent, string? mfaCode = null, CancellationToken ct = default);
    Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> RefreshTokenAsync(string refreshToken, string ipAddress, string userAgent, CancellationToken ct = default);
    Task<bool> RevokeSessionAsync(string tokenJti, string motivo, CancellationToken ct = default);
    Task<bool> ValidateMfaAsync(Guid userId, string code, CancellationToken ct = default);
    Task<(string Secret, string ProvisioningUri)> SetupMfaAsync(Guid userId, CancellationToken ct = default);
    Task LogoutAsync(Guid userId, string? tokenJti = null, CancellationToken ct = default);
}

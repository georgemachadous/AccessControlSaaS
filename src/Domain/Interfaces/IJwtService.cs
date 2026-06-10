namespace AccessControl.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, Guid tenantId, IEnumerable<string> roles, IEnumerable<string> permissions, string? jti = null);
    string GenerateRefreshToken();
    bool ValidateToken(string token, out Dictionary<string, object> claims);
    Guid? GetUserIdFromToken(string token);
    Guid? GetTenantIdFromToken(string token);
}

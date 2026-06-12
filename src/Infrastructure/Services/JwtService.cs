using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AccessControl.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AccessControl.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(Guid userId, string email, Guid tenantId, IEnumerable<string> roles, IEnumerable<string> permissions, IDictionary<string, object>? extraClaims = null, string? jti = null)
    {
        var key = _config["Jwt:PrivateKey"] ?? "chave-minima-32-caracteres-para-dev";
        var issuer = _config["Jwt:Issuer"] ?? "AccessControlSaaS";
        var audience = _config["Jwt:Audience"] ?? "AccessControlSaaS-Client";
        var expiresInMinutes = int.TryParse(_config["Jwt:AccessTokenMinutes"], out var m) ? m : 15;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("tenant", tenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        // permissions contain app:{appId}:... entries and will be added as custom claims
        claims.AddRange(permissions.Select(p => new Claim("appperm", p)));

        if (extraClaims is not null)
        {
            foreach (var kv in extraClaims)
            {
                // If value is a list/array, store as JSON string claim
                if (kv.Value is IEnumerable<string> arr)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(arr);
                    claims.Add(new Claim(kv.Key, json));
                }
                else
                {
                    claims.Add(new Claim(kv.Key, kv.Value?.ToString() ?? string.Empty));
                }
            }
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddMinutes(expiresInMinutes), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid();
    }

    public bool ValidateToken(string token, out Dictionary<string, object> claims)
    {
        claims = new Dictionary<string, object>();
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            foreach (var c in jwt.Claims)
            {
                claims[c.Type] = c.Value;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Guid? GetUserIdFromToken(string token)
    {
        if (ValidateToken(token, out var claims) && claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub))
        {
            return Guid.TryParse(sub?.ToString(), out var id) ? id : null;
        }
        return null;
    }

    public Guid? GetTenantIdFromToken(string token)
    {
        if (ValidateToken(token, out var claims) && claims.TryGetValue("tenant", out var t))
        {
            return Guid.TryParse(t?.ToString(), out var id) ? id : null;
        }
        return null;
    }
}

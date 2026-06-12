using AccessControl.Domain.Interfaces;
using OtpNet;
using AccessControl.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AccessControl.Domain.Entities;

namespace AccessControl.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AccessControlDbContext _db;
    private readonly IJwtService _jwt;

    public AuthService(AccessControlDbContext db, IJwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> AuthenticateAsync(string email, string password, string ipAddress, string userAgent, string? mfaCode = null, CancellationToken ct = default)
    {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null) throw new UnauthorizedAccessException("Invalid credentials");

        // Lockout check
        if (user.BloqueadoAte.HasValue && user.BloqueadoAte.Value > DateTime.UtcNow)
            throw new UnauthorizedAccessException("User is locked out");

        // Validate password
        var valid = BCrypt.Net.BCrypt.Verify(password, user.SenhaHash);
        if (!valid)
        {
            user.TentativasFalhasLogin++;
            if (user.TentativasFalhasLogin >= 5)
            {
                user.BloqueadoAte = DateTime.UtcNow.AddMinutes(30);
                user.TentativasFalhasLogin = 0;
            }
            await _db.SaveChangesAsync(ct);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // If user has MFA enabled, validate code
        if (user.MfaHabilitado)
        {
            if (string.IsNullOrEmpty(mfaCode) || !await ValidateMfaAsync(user.Id, mfaCode, ct))
                throw new UnauthorizedAccessException("MFA required or invalid code");
        }

        // Reset failed attempts on successful login
        user.TentativasFalhasLogin = 0;
        user.BloqueadoAte = null;
        user.UltimoAcesso = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        var roles = user.UsuarioPerfilAplicacoes.Select(u => u.Perfil?.Nome ?? "Usuario").Distinct();

        // Build per-application claims: profile and features
        var permissions = new List<string>();
        var perfilGroups = user.UsuarioPerfilAplicacoes.GroupBy(u => u.AplicacaoId);
        var extraClaims = new Dictionary<string, object>();
        foreach (var g in perfilGroups)
        {
            var appId = g.Key;
            var perfilNames = g.Select(x => x.Perfil?.Nome).Where(n => n != null).Distinct().ToList()!;
            if (perfilNames.Any()) extraClaims[$"app:{appId}:profiles"] = perfilNames;

            var features = g.SelectMany(x => x.Perfil?.PerfilFuncionalidades ?? new List<PerfilFuncionalidade>())
                            .Select(pf => pf.Funcionalidade?.Codigo)
                            .Where(c => c != null)
                            .Distinct()
                            .ToList()!;
            if (features.Any()) extraClaims[$"app:{appId}:features"] = features;
        }

        var jti = Guid.NewGuid().ToString();
        var token = _jwt.GenerateToken(user.Id, user.Email, user.EmpresaId ?? Guid.Empty, roles, permissions, extraClaims, jti);
        var refresh = _jwt.GenerateRefreshToken();

        var session = new SessaoUsuario
        {
            Id = Guid.NewGuid(),
            UsuarioId = user.Id,
            TokenJti = jti,
            RefreshToken = refresh,
            DataCriacao = DateTime.UtcNow,
            DataExpiracao = DateTime.UtcNow.AddDays(7),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsValida = true
        };
        _db.SessoesUsuarios.Add(session);
        await _db.SaveChangesAsync(ct);

        return (token, refresh, DateTime.UtcNow.AddMinutes(15));
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> RefreshTokenAsync(string refreshToken, string ipAddress, string userAgent, CancellationToken ct = default)
    {
        var session = await _db.SessoesUsuarios.Include(s => s.Usuario).FirstOrDefaultAsync(s => s.RefreshToken == refreshToken && s.IsValida, ct);
        if (session is null) throw new UnauthorizedAccessException("Invalid refresh token");
        if (session.DataExpiracao < DateTime.UtcNow) throw new UnauthorizedAccessException("Refresh token expired");

        var user = session.Usuario;
        var roles = user.UsuarioPerfilAplicacoes.Select(u => u.Perfil?.Nome ?? "Usuario").Distinct();
        var permissions = new List<string>();

        // per-app structured claims
        var perfilGroups = user.UsuarioPerfilAplicacoes.GroupBy(u => u.AplicacaoId);
        var extraClaimsRefresh = new Dictionary<string, object>();
        foreach (var g in perfilGroups)
        {
            var appId = g.Key;
            var perfilNames = g.Select(x => x.Perfil?.Nome).Where(n => n != null).Distinct().ToList()!;
            if (perfilNames.Any()) extraClaimsRefresh[$"app:{appId}:profiles"] = perfilNames;

            var features = g.SelectMany(x => x.Perfil?.PerfilFuncionalidades ?? new List<PerfilFuncionalidade>())
                            .Select(pf => pf.Funcionalidade?.Codigo)
                            .Where(c => c != null)
                            .Distinct()
                            .ToList()!;
            if (features.Any()) extraClaimsRefresh[$"app:{appId}:features"] = features;
        }

        var jti = Guid.NewGuid().ToString();
        var token = _jwt.GenerateToken(user.Id, user.Email, user.EmpresaId ?? Guid.Empty, roles, permissions, extraClaimsRefresh, jti);
        var refresh = _jwt.GenerateRefreshToken();

        session.TokenJti = jti;
        session.RefreshToken = refresh;
        session.DataCriacao = DateTime.UtcNow;
        session.DataExpiracao = DateTime.UtcNow.AddDays(7);
        session.DataUltimaAtividade = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return (token, refresh, DateTime.UtcNow.AddMinutes(15));
    }

    public Task<bool> RevokeSessionAsync(string tokenJti, string motivo, CancellationToken ct = default)
    {
        var session = _db.SessoesUsuarios.FirstOrDefault(s => s.TokenJti == tokenJti);
        if (session is null) return Task.FromResult(false);
        session.IsValida = false;
        session.MotivoInvalidacao = motivo;
        session.DataInvalidacao = DateTime.UtcNow;
        _db.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<bool> ValidateMfaAsync(Guid userId, string code, CancellationToken ct = default)
    {
        var user = _db.Usuarios.FirstOrDefault(u => u.Id == userId);
        if (user is null || string.IsNullOrEmpty(user.MfaSecretKey)) return Task.FromResult(false);
        try
        {
            var bytes = Base32Encoding.ToBytes(user.MfaSecretKey);
            var totp = new OtpNet.Totp(bytes);
            var valid = totp.VerifyTotp(code, out long _, new VerificationWindow(2, 2));
            if (valid && !user.MfaHabilitado)
            {
                user.MfaHabilitado = true;
                _db.SaveChanges();
            }
            return Task.FromResult(valid);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<(string Secret, string ProvisioningUri)> SetupMfaAsync(Guid userId, CancellationToken ct = default)
    {
        var user = _db.Usuarios.FirstOrDefault(u => u.Id == userId);
        if (user is null) return Task.FromResult<(string, string)>((string.Empty, string.Empty));

        var secret = OtpNet.KeyGeneration.GenerateRandomKey(20);
        var secretBase32 = Base32Encoding.ToString(secret);
        user.MfaSecretKey = secretBase32;
        user.MfaHabilitado = false; // enable after verification
        _db.SaveChanges();

        var issuer = "AccessControlSaaS";
        // Generate otpauth provisioning URI compatible with authenticator apps
        // Build otpauth provisioning URI manually (compatible with Authenticator apps)
        var label = Uri.EscapeDataString($"{issuer}:{user.Email}");
        var issuerEnc = Uri.EscapeDataString(issuer);
        var provisioningUri = $"otpauth://totp/{label}?secret={secretBase32}&issuer={issuerEnc}&algorithm=SHA1&digits=6&period=30";
        return Task.FromResult((secretBase32, provisioningUri));
    }

    public Task LogoutAsync(Guid userId, string? tokenJti = null, CancellationToken ct = default)
    {
        var sessions = _db.SessoesUsuarios.Where(s => s.UsuarioId == userId && (tokenJti == null || s.TokenJti == tokenJti));
        foreach (var s in sessions)
        {
            s.IsValida = false;
            s.DataInvalidacao = DateTime.UtcNow;
            s.MotivoInvalidacao = "User logout";
        }
        _db.SaveChanges();
        return Task.CompletedTask;
    }
}

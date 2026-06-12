using System.Text.Json;
using System.Net.Http.Headers;
using AccessControl.Domain.Interfaces;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AccessControl.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AccessControl.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace AccessControl.Infrastructure.Services;

public class SsoService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly AccessControlDbContext _db;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _config;

    public SsoService(IHttpClientFactory httpFactory, AccessControlDbContext db, IJwtService jwt, IConfiguration config)
    {
        _httpFactory = httpFactory;
        _db = db;
        _jwt = jwt;
        _config = config;
    }

    private async Task ValidateIdTokenAsync(string idToken, string wellKnownUrl, CancellationToken ct = default)
    {
        // Cache configuration per provider to reduce roundtrips
        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(wellKnownUrl, new OpenIdConnectConfigurationRetriever());
        var config = await configManager.GetConfigurationAsync(ct);
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = new[] { config.Issuer },
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKeys = config.SigningKeys,
            RequireSignedTokens = true
        };

        tokenHandler.ValidateToken(idToken, validationParameters, out var validatedToken);
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> HandleGoogleAsync(string code, string redirectUri, CancellationToken ct = default)
    {
        var client = _httpFactory.CreateClient();
        var tokenEndpoint = "https://oauth2.googleapis.com/token";
        var req = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _config["Sso:GoogleClientId"] ?? _config["Authentication:Google:ClientId"] ?? "" },
            { "client_secret", _config["Sso:GoogleClientSecret"] ?? _config["Authentication:Google:ClientSecret"] ?? "" },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        };

        var res = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(req), ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("id_token", out var idTokenElement) || idTokenElement.ValueKind == JsonValueKind.Null)
        {
            throw new UnauthorizedAccessException("id_token missing from provider response");
        }
        var idToken = idTokenElement.GetString();

        // validate id_token signature using provider metadata
        try
        {
            await ValidateIdTokenAsync(idToken!, "https://accounts.google.com/.well-known/openid-configuration", ct);
        }
        catch
        {
            // if validation fails, reject
            throw new UnauthorizedAccessException("Invalid id_token");
        }

        // decode id_token payload
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
        {
            user = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = name ?? email ?? "Google User",
                Email = email ?? string.Empty,
                IsSsoUser = true,
                SsoProvider = "google",
                SsoExternalId = sub,
                Ativo = true
            };
            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        // Issue tokens
        var roles = user.UsuarioPerfilAplicacoes.Select(x => x.Perfil?.Nome ?? "Usuario").Distinct();
        var permissions = new List<string>();
        var perfilGroups = user.UsuarioPerfilAplicacoes.GroupBy(u => u.AplicacaoId);
        var extraClaims = new Dictionary<string, object>();
        foreach (var g in perfilGroups)
        {
            var appId = g.Key;
            var perfilNames = g.Select(x => x.Perfil?.Nome).Where(n => n != null).Distinct().ToList();
            if (perfilNames.Any()) extraClaims[$"app:{appId}:profiles"] = perfilNames;

            var features = g.SelectMany(x => x.Perfil?.PerfilFuncionalidades ?? new List<PerfilFuncionalidade>())
                            .Select(pf => pf.Funcionalidade?.Codigo)
                            .Where(c => c != null)
                            .Distinct()
                            .ToList();
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
            IpAddress = "sso",
            UserAgent = "sso",
            IsValida = true
        };
        _db.SessoesUsuarios.Add(session);
        await _db.SaveChangesAsync(ct);

        return (token, refresh, DateTime.UtcNow.AddMinutes(15));
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> HandleMicrosoftAsync(string code, string redirectUri, CancellationToken ct = default)
    {
        // Simplified placeholder implementation — implement real token exchange and userinfo retrieval
        var client = _httpFactory.CreateClient();
        // Microsoft token endpoint
        var tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        var req = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _config["Sso:MicrosoftClientId"] ?? _config["Authentication:Microsoft:ClientId"] ?? "" },
            { "client_secret", _config["Sso:MicrosoftClientSecret"] ?? _config["Authentication:Microsoft:ClientSecret"] ?? "" },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        };

        var res = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(req), ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("id_token", out var idTokenElement) || idTokenElement.ValueKind == JsonValueKind.Null)
        {
            throw new UnauthorizedAccessException("id_token missing from provider response");
        }
        var idToken = idTokenElement.GetString();

        try
        {
            await ValidateIdTokenAsync(idToken!, "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration", ct);
        }
        catch
        {
            throw new UnauthorizedAccessException("Invalid id_token");
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        var email = jwt.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
        {
            user = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = name ?? email ?? "Microsoft User",
                Email = email ?? string.Empty,
                IsSsoUser = true,
                SsoProvider = "microsoft",
                SsoExternalId = sub,
                Ativo = true
            };
            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        var roles = user.UsuarioPerfilAplicacoes.Select(x => x.Perfil?.Nome ?? "Usuario").Distinct();
        var permissions = new List<string>();
        var jti = Guid.NewGuid().ToString();
        var token = _jwt.GenerateToken(user.Id, user.Email, user.EmpresaId ?? Guid.Empty, roles, permissions, null, jti);
        var refresh = _jwt.GenerateRefreshToken();

        var session = new SessaoUsuario
        {
            Id = Guid.NewGuid(),
            UsuarioId = user.Id,
            TokenJti = jti,
            RefreshToken = refresh,
            DataCriacao = DateTime.UtcNow,
            DataExpiracao = DateTime.UtcNow.AddDays(7),
            IpAddress = "sso",
            UserAgent = "sso",
            IsValida = true
        };
        _db.SessoesUsuarios.Add(session);
        await _db.SaveChangesAsync(ct);

        return (token, refresh, DateTime.UtcNow.AddMinutes(15));
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> HandleGithubAsync(string code, string redirectUri, CancellationToken ct = default)
    {
        var client = _httpFactory.CreateClient();
        var tokenEndpoint = "https://github.com/login/oauth/access_token";
        var req = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _config["Sso:GithubClientId"] ?? "" },
            { "client_secret", _config["Sso:GithubClientSecret"] ?? "" },
            { "redirect_uri", redirectUri }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(req)
        };
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.UserAgent.ParseAdd("AccessControlSaaS/1.0");
        var res = await client.SendAsync(request, ct);
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync(ct);
        // GitHub returns query string style response like: access_token=...&scope=...&token_type=bearer
        var accessToken = content.Split('&').Select(p => p.Split('=')).Where(parts => parts.Length == 2 && parts[0] == "access_token").Select(parts => parts[1]).FirstOrDefault();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
        var userRes = await client.GetAsync("https://api.github.com/user", ct);
        userRes.EnsureSuccessStatusCode();
        var userJson = await userRes.Content.ReadAsStringAsync(ct);
        using var userDoc = JsonDocument.Parse(userJson);
        var email = userDoc.RootElement.GetProperty("email").GetString() ?? userDoc.RootElement.GetProperty("login").GetString();
        var sub = userDoc.RootElement.GetProperty("id").GetInt32().ToString();
        var name = userDoc.RootElement.GetProperty("name").GetString() ?? email;

        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
        {
            user = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = name ?? email ?? "GitHub User",
                Email = email ?? string.Empty,
                IsSsoUser = true,
                SsoProvider = "github",
                SsoExternalId = sub,
                Ativo = true
            };
            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        var roles = user.UsuarioPerfilAplicacoes.Select(x => x.Perfil?.Nome ?? "Usuario").Distinct();
        var permissions = new List<string>();
        var jti = Guid.NewGuid().ToString();
        var token = _jwt.GenerateToken(user.Id, user.Email, user.EmpresaId ?? Guid.Empty, roles, permissions, null, jti);
        var refresh = _jwt.GenerateRefreshToken();

        var session = new SessaoUsuario
        {
            Id = Guid.NewGuid(),
            UsuarioId = user.Id,
            TokenJti = jti,
            RefreshToken = refresh,
            DataCriacao = DateTime.UtcNow,
            DataExpiracao = DateTime.UtcNow.AddDays(7),
            IpAddress = "sso",
            UserAgent = "sso",
            IsValida = true
        };
        _db.SessoesUsuarios.Add(session);
        await _db.SaveChangesAsync(ct);

        return (token, refresh, DateTime.UtcNow.AddMinutes(15));
    }
}

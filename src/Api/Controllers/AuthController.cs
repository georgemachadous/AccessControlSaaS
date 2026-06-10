using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Interfaces;

namespace AccessControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, IAuthService authService, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var command = new LoginCommand(request, ipAddress, userAgent);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken ct)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var command = new RefreshTokenCommand(request, ipAddress, userAgent);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var tokenJti = User.FindFirst("jti")?.Value;

        if (userId is null) return Unauthorized();

        var command = new LogoutCommand(Guid.Parse(userId), tokenJti);
        await _mediator.Send(command, ct);

        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] AlterarSenhaDto request, CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var command = new AlterarSenhaCommand(Guid.Parse(userId), request);
        var result = await _mediator.Send(command, ct);
        return result ? Ok(new { message = "Password changed successfully" }) : BadRequest(new { message = "Failed to change password" });
    }

    [HttpPost("recover-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverPassword([FromBody] RecuperarSenhaDto request, CancellationToken ct)
    {
        var command = new RecuperarSenhaCommand(request);
        await _mediator.Send(command, ct);
        // Always return OK to prevent user enumeration
        return Ok(new { message = "If the email exists, a recovery link has been sent" });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] RedefinirSenhaDto request, CancellationToken ct)
    {
        var command = new RedefinirSenhaCommand(request);
        var result = await _mediator.Send(command, ct);
        return result ? Ok(new { message = "Password reset successfully" }) : BadRequest(new { message = "Invalid or expired token" });
    }

    [HttpGet("sso/{provider}")]
    [AllowAnonymous]
    public IActionResult SsoLogin(string provider, [FromQuery] string? redirectUri = null)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri ?? "/api/auth/sso-callback",
            Items = { { "provider", provider } }
        };

        return Challenge(properties, provider);
    }

    [HttpGet("sso-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> SsoCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            _logger.LogWarning("SSO authentication failed");
            return Unauthorized(new { message = "SSO authentication failed" });
        }

        var email = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var provider = result.Properties?.Items["provider"] ?? "unknown";
        var externalId = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (email is null) return BadRequest(new { message = "Email not provided by SSO provider" });

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var ssoRequest = new SsoLoginRequestDto(provider, "", null, "");
        var command = new SsoLoginCommand(ssoRequest, ipAddress, userAgent);
        // In real implementation, handle SSO user creation/login

        return Ok(new { message = "SSO login successful", email, name });
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsuarioDto>> GetCurrentUser(CancellationToken ct)
    {
        var query = new AccessControl.Application.Queries.GetCurrentUserQuery();
        var result = await _mediator.Send(query, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost("mfa/setup")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetupMfa(CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var result = await _authService.SetupMfaAsync(Guid.Parse(userId), ct);
        return result ? Ok(new { message = "MFA setup initiated" }) : BadRequest(new { message = "Failed to setup MFA" });
    }

    [HttpPost("mfa/validate")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateMfa([FromBody] string code, CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var result = await _authService.ValidateMfaAsync(Guid.Parse(userId), code, ct);
        return result ? Ok(new { message = "MFA validated successfully" }) : BadRequest(new { message = "Invalid MFA code" });
    }
}

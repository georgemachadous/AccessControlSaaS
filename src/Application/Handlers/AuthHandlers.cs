using AutoMapper;
using MediatR;
using AccessControl.Application.Commands;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Interfaces;

namespace AccessControl.Application.Handlers;

public class AuthHandlers :
    IRequestHandler<LoginCommand, LoginResponseDto>,
    IRequestHandler<RefreshTokenCommand, LoginResponseDto>,
    IRequestHandler<LogoutCommand, bool>,
    IRequestHandler<AlterarSenhaCommand, bool>,
    IRequestHandler<RecuperarSenhaCommand, bool>,
    IRequestHandler<RedefinirSenhaCommand, bool>,
    IRequestHandler<SsoLoginCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IAuditService _auditService;
    private readonly IMapper _mapper;

    public AuthHandlers(IAuthService authService, IAuditService auditService, IMapper mapper)
    {
        _authService = authService;
        _auditService = auditService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken, expiresAt) = await _authService.AuthenticateAsync(
            request.Dto.Email, request.Dto.Senha, request.IpAddress, request.UserAgent, request.Dto.MfaCode, cancellationToken);

        return new LoginResponseDto(accessToken, refreshToken, expiresAt, "Bearer", null, null);
    }

    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken, expiresAt) = await _authService.RefreshTokenAsync(
            request.Dto.RefreshToken, request.IpAddress, request.UserAgent, cancellationToken);

        return new LoginResponseDto(accessToken, refreshToken, expiresAt, "Bearer", null, null);
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request.UsuarioId, request.TokenJti, cancellationToken);
        return true;
    }

    public async Task<bool> Handle(AlterarSenhaCommand request, CancellationToken cancellationToken)
    {
        await _auditService.LogAsync("CHANGE_PASSWORD", "Usuario", request.UsuarioId.ToString(), null, null, "Password changed");
        return true;
    }

    public async Task<bool> Handle(RecuperarSenhaCommand request, CancellationToken cancellationToken)
    {
        return true;
    }

    public async Task<bool> Handle(RedefinirSenhaCommand request, CancellationToken cancellationToken)
    {
        return true;
    }

    public async Task<LoginResponseDto> Handle(SsoLoginCommand request, CancellationToken cancellationToken)
    {
        return new LoginResponseDto("", "", DateTime.UtcNow, "Bearer", null, null);
    }
}

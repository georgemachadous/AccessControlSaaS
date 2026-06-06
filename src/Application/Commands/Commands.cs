using MediatR;
using AccessControl.Application.DTOs;

namespace AccessControl.Application.Commands;

// Empresa Commands
public record CriarEmpresaCommand(CriarEmpresaDto Dto) : IRequest<EmpresaDto>;
public record AtualizarEmpresaCommand(AtualizarEmpresaDto Dto) : IRequest<EmpresaDto>;
public record ExcluirEmpresaCommand(Guid Id) : IRequest<bool>;

// Filial Commands
public record CriarFilialCommand(CriarFilialDto Dto) : IRequest<FilialDto>;
public record AtualizarFilialCommand(AtualizarFilialDto Dto) : IRequest<FilialDto>;
public record ExcluirFilialCommand(Guid Id) : IRequest<bool>;

// Usuario Commands
public record CriarUsuarioCommand(CriarUsuarioDto Dto) : IRequest<UsuarioDto>;
public record AtualizarUsuarioCommand(AtualizarUsuarioDto Dto) : IRequest<UsuarioDto>;
public record ExcluirUsuarioCommand(Guid Id) : IRequest<bool>;
public record AtivarUsuarioCommand(Guid Id) : IRequest<bool>;
public record DesativarUsuarioCommand(Guid Id) : IRequest<bool>;

// Perfil Commands
public record CriarPerfilCommand(CriarPerfilDto Dto) : IRequest<PerfilDto>;
public record AtualizarPerfilCommand(AtualizarPerfilDto Dto) : IRequest<PerfilDto>;
public record ExcluirPerfilCommand(Guid Id) : IRequest<bool>;

// Funcionalidade Commands
public record CriarFuncionalidadeCommand(CriarFuncionalidadeDto Dto) : IRequest<FuncionalidadeDto>;
public record AtualizarFuncionalidadeCommand(AtualizarFuncionalidadeDto Dto) : IRequest<FuncionalidadeDto>;
public record ExcluirFuncionalidadeCommand(Guid Id) : IRequest<bool>;

// Aplicacao Commands
public record CriarAplicacaoCommand(CriarAplicacaoDto Dto) : IRequest<AplicacaoDto>;
public record AtualizarAplicacaoCommand(AtualizarAplicacaoDto Dto) : IRequest<AplicacaoDto>;
public record ExcluirAplicacaoCommand(Guid Id) : IRequest<bool>;

// Auth Commands
public record LoginCommand(LoginRequestDto Dto, string IpAddress, string UserAgent) : IRequest<LoginResponseDto>;
public record RefreshTokenCommand(RefreshTokenRequestDto Dto, string IpAddress, string UserAgent) : IRequest<LoginResponseDto>;
public record LogoutCommand(Guid UsuarioId, string? TokenJti) : IRequest<bool>;
public record AlterarSenhaCommand(Guid UsuarioId, AlterarSenhaDto Dto) : IRequest<bool>;
public record RecuperarSenhaCommand(RecuperarSenhaDto Dto) : IRequest<bool>;
public record RedefinirSenhaCommand(RedefinirSenhaDto Dto) : IRequest<bool>;
public record SsoLoginCommand(SsoLoginRequestDto Dto, string IpAddress, string UserAgent) : IRequest<LoginResponseDto>;

// PerfilFuncionalidade Commands
public record AtribuirFuncionalidadeCommand(AtribuirFuncionalidadeDto Dto) : IRequest<PerfilFuncionalidadeDto>;
public record RemoverFuncionalidadeCommand(Guid PerfilId, Guid FuncionalidadeId) : IRequest<bool>;

// UsuarioPerfilAplicacao Commands
public record AtribuirPerfilCommand(AtribuirPerfilDto Dto) : IRequest<UsuarioPerfilAplicacaoDto>;
public record RemoverPerfilUsuarioCommand(Guid UsuarioId, Guid PerfilId, Guid AplicacaoId) : IRequest<bool>;

// Notificacao Commands
public record CriarNotificacaoCommand(CriarNotificacaoDto Dto) : IRequest<NotificacaoDto>;
public record MarcarNotificacaoLidaCommand(Guid NotificacaoId) : IRequest<bool>;

// Upload Commands
public record UploadLogoEmpresaCommand(Guid EmpresaId, byte[] FileData, string FileName, string ContentType) : IRequest<string>;
public record UploadAvatarUsuarioCommand(Guid UsuarioId, byte[] FileData, string FileName, string ContentType) : IRequest<string>;

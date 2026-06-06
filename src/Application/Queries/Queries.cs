using MediatR;
using AccessControl.Application.DTOs;

namespace AccessControl.Application.Queries;

// Empresa Queries
public record GetEmpresaByIdQuery(Guid Id) : IRequest<EmpresaDto?>;
public record GetAllEmpresasQuery(int Page = 1, int PageSize = 20, string? Search = null, bool? Ativa = null) : IRequest<PaginatedResult<EmpresaDto>>;
public record GetEmpresaStatsQuery(Guid Id) : IRequest<DashboardStatsDto>;

// Filial Queries
public record GetFilialByIdQuery(Guid Id) : IRequest<FilialDto?>;
public record GetFiliaisByEmpresaQuery(Guid EmpresaId, int Page = 1, int PageSize = 20) : IRequest<PaginatedResult<FilialDto>>;

// Usuario Queries
public record GetUsuarioByIdQuery(Guid Id) : IRequest<UsuarioDto?>;
public record GetUsuarioByEmailQuery(string Email) : IRequest<UsuarioDto?>;
public record GetAllUsuariosQuery(int Page = 1, int PageSize = 20, string? Search = null, Guid? EmpresaId = null, Guid? FilialId = null, bool? Ativo = null) : IRequest<PaginatedResult<UsuarioDto>>;
public record GetUsuariosByPerfilQuery(Guid PerfilId) : IRequest<IEnumerable<UsuarioDto>>;
public record GetUsuariosByAplicacaoQuery(Guid AplicacaoId) : IRequest<IEnumerable<UsuarioDto>>;

// Perfil Queries
public record GetPerfilByIdQuery(Guid Id) : IRequest<PerfilDto?>;
public record GetPerfisByEmpresaQuery(Guid EmpresaId, int Page = 1, int PageSize = 20) : IRequest<PaginatedResult<PerfilDto>>;
public record GetPerfisByUsuarioAplicacaoQuery(Guid UsuarioId, Guid AplicacaoId) : IRequest<IEnumerable<PerfilDto>>;

// Funcionalidade Queries
public record GetFuncionalidadeByIdQuery(Guid Id) : IRequest<FuncionalidadeDto?>;
public record GetAllFuncionalidadesQuery(int Page = 1, int PageSize = 50, string? Search = null, Guid? AplicacaoId = null) : IRequest<PaginatedResult<FuncionalidadeDto>>;
public record GetFuncionalidadesByPerfilQuery(Guid PerfilId) : IRequest<IEnumerable<PerfilFuncionalidadeDto>>;
public record GetFuncionalidadesByUsuarioQuery(Guid UsuarioId, Guid AplicacaoId) : IRequest<IEnumerable<FuncionalidadeDto>>;

// Aplicacao Queries
public record GetAplicacaoByIdQuery(Guid Id) : IRequest<AplicacaoDto?>;
public record GetAllAplicacoesQuery(int Page = 1, int PageSize = 20, string? Search = null, bool? Ativa = null) : IRequest<PaginatedResult<AplicacaoDto>>;
public record GetAplicacoesByEmpresaQuery(Guid EmpresaId) : IRequest<IEnumerable<AplicacaoDto>>;
public record GetAplicacoesByUsuarioQuery(Guid UsuarioId) : IRequest<IEnumerable<AplicacaoDto>>;

// Auth Queries
public record GetCurrentUserQuery() : IRequest<UsuarioDto?>;
public record GetUserPermissionsQuery(Guid UsuarioId, Guid AplicacaoId) : IRequest<IEnumerable<string>>;
public record GetUserSessionsQuery(Guid UsuarioId) : IRequest<IEnumerable<Domain.Entities.SessaoUsuario>>;
public record ValidateTokenQuery(string Token) : IRequest<bool>;

// Notificacao Queries
public record GetNotificacoesByUsuarioQuery(Guid UsuarioId, bool? Lida = null, int Page = 1, int PageSize = 20) : IRequest<PaginatedResult<NotificacaoDto>>;
public record GetNotificacoesNaoLidasCountQuery(Guid UsuarioId) : IRequest<int>;

// Dashboard Queries
public record GetDashboardStatsQuery() : IRequest<DashboardStatsDto>;
public record GetLoginsPorDiaQuery(int Dias = 30) : IRequest<IEnumerable<LoginPorDiaDto>>;
public record GetAuditoriaQuery(string? Entidade = null, string? Acao = null, DateTime? From = null, DateTime? To = null, int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<Domain.Entities.LogAuditoria>>;

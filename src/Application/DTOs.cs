namespace AccessControl.Application.DTOs;

// Empresa DTOs
public record EmpresaDto(Guid Id, string Nome, string NomeFantasia, string Cnpj, string Email, string Telefone, string LogoUrl, string IdiomaPadrao, bool Ativa, string Cidade, string Estado, string Pais, DateTime CreatedAt);
public record CriarEmpresaDto(string Nome, string NomeFantasia, string Cnpj, string Email, string Telefone, string IdiomaPadrao, string Cep, string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Pais);
public record AtualizarEmpresaDto(Guid Id, string Nome, string NomeFantasia, string Email, string Telefone, string IdiomaPadrao, string Cep, string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Pais, bool Ativa);

// Filial DTOs
public record FilialDto(Guid Id, Guid EmpresaId, string Nome, string Codigo, string Idioma, bool Ativa, string Cidade, string Estado, string Pais, DateTime CreatedAt);
public record CriarFilialDto(Guid EmpresaId, string Nome, string Codigo, string Idioma, string Cep, string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Pais);
public record AtualizarFilialDto(Guid Id, string Nome, string Codigo, string Idioma, string Cep, string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Pais, bool Ativa);

// Usuario DTOs
public record UsuarioDto(Guid Id, string Nome, string Email, string? Telefone, string? Cpf, string Idioma, string? AvatarUrl, bool Ativo, bool EmailConfirmado, bool MfaHabilitado, DateTime? UltimoAcesso, Guid? FilialId, DateTime CreatedAt);
public record CriarUsuarioDto(string Nome, string Email, string? Telefone, string? Cpf, string Idioma, Guid? FilialId, string Senha, bool IsSsoUser, string? SsoProvider, string? SsoExternalId);
public record AtualizarUsuarioDto(Guid Id, string Nome, string Email, string? Telefone, string? Cpf, string Idioma, Guid? FilialId, bool Ativo, bool MfaHabilitado);

// Perfil DTOs
public record PerfilDto(Guid Id, Guid EmpresaId, string Nome, string Descricao, bool IsPadrao, bool Ativo, int TotalUsuarios, DateTime CreatedAt);
public record CriarPerfilDto(Guid EmpresaId, string Nome, string Descricao, bool IsPadrao);
public record AtualizarPerfilDto(Guid Id, string Nome, string Descricao, bool IsPadrao, bool Ativo);

// Funcionalidade DTOs
public record FuncionalidadeDto(Guid Id, string Nome, string Descricao, string Codigo, string Categoria, string? Icone, string? Rota, int Ordem, bool Ativa, bool IsPadrao, Guid? AplicacaoId);
public record CriarFuncionalidadeDto(string Nome, string Descricao, string Codigo, string Categoria, string? Icone, string? Rota, int Ordem, Guid? AplicacaoId);
public record AtualizarFuncionalidadeDto(Guid Id, string Nome, string Descricao, string Codigo, string Categoria, string? Icone, string? Rota, int Ordem, bool Ativa);

// Aplicacao DTOs
public record AplicacaoDto(Guid Id, string Nome, string Descricao, string Codigo, string Url, string? Icone, string? Cor, bool Ativa, bool IsPublica, DateTime CreatedAt);
public record CriarAplicacaoDto(string Nome, string Descricao, string Codigo, string Url, string? Icone, string? Cor, bool IsPublica, string? SsoClientId, string? SsoAuthority, string? SsoScopes);
public record AtualizarAplicacaoDto(Guid Id, string Nome, string Descricao, string Url, string? Icone, string? Cor, bool Ativa, bool IsPublica);

// Auth DTOs
public record LoginRequestDto(string Email, string Senha, bool? LembrarMe);
public record LoginResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, string TokenType, string? MfaRequired, UsuarioDto? Usuario);
public record RefreshTokenRequestDto(string RefreshToken);
public record SsoLoginRequestDto(string Provider, string Code, string? State, string RedirectUri);
public record AlterarSenhaDto(string SenhaAtual, string NovaSenha, string ConfirmarNovaSenha);
public record RecuperarSenhaDto(string Email);
public record RedefinirSenhaDto(string Token, string NovaSenha, string ConfirmarNovaSenha);

// PerfilFuncionalidade DTOs
public record AtribuirFuncionalidadeDto(Guid PerfilId, Guid FuncionalidadeId, bool PermiteCriar, bool PermiteLer, bool PermiteAtualizar, bool PermiteDeletar, bool PermiteExecutar);
public record PerfilFuncionalidadeDto(Guid PerfilId, Guid FuncionalidadeId, string NomeFuncionalidade, string CodigoFuncionalidade, bool PermiteCriar, bool PermiteLer, bool PermiteAtualizar, bool PermiteDeletar, bool PermiteExecutar);

// UsuarioPerfilAplicacao DTOs
public record AtribuirPerfilDto(Guid UsuarioId, Guid PerfilId, Guid AplicacaoId, bool IsPrincipal, DateTime? DataExpiracao);
public record UsuarioPerfilAplicacaoDto(Guid UsuarioId, string NomeUsuario, Guid PerfilId, string NomePerfil, Guid AplicacaoId, string NomeAplicacao, bool IsPrincipal, DateTime? DataExpiracao);

// Notificacao DTOs
public record NotificacaoDto(Guid Id, string Titulo, string Mensagem, string Tipo, string? Link, bool Lida, DateTime? DataLeitura, DateTime CreatedAt);
public record CriarNotificacaoDto(Guid UsuarioId, string Titulo, string Mensagem, string Tipo, string? Link, bool EnviarPush, bool EnviarEmail);

// Dashboard/Stats DTOs
public record DashboardStatsDto(int TotalEmpresas, int TotalUsuarios, int TotalAplicacoes, int TotalPerfis, int LoginsHoje, int UsuariosAtivos, int SessoesAtivas, List<LoginPorDiaDto> LoginsPorDia);
public record LoginPorDiaDto(DateTime Data, int Quantidade);

// Paginated Result
public record PaginatedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize, int TotalPages);

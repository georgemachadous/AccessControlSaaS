using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControl.Infrastructure.flyway.Initial_20260611074442
{
    /// <inheritdoc />
    public partial class Initial_20260611074442 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aplicacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Icone = table.Column<string>(type: "TEXT", nullable: true),
                    Cor = table.Column<string>(type: "TEXT", nullable: true),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublica = table.Column<bool>(type: "INTEGER", nullable: false),
                    SsoClientId = table.Column<string>(type: "TEXT", nullable: true),
                    SsoClientSecret = table.Column<string>(type: "TEXT", nullable: true),
                    SsoAuthority = table.Column<string>(type: "TEXT", nullable: true),
                    SsoScopes = table.Column<string>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracoesSistema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Chave = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    IsPublico = table.Column<bool>(type: "INTEGER", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    NomeFantasia = table.Column<string>(type: "TEXT", nullable: false),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IdiomaPadrao = table.Column<string>(type: "TEXT", nullable: false),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cep = table.Column<string>(type: "TEXT", nullable: false),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Complemento = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Pais = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "BR"),
                    ConfiguracoesJSON = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "{}"),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Acao = table.Column<string>(type: "TEXT", nullable: false),
                    Entidade = table.Column<string>(type: "TEXT", nullable: false),
                    EntidadeId = table.Column<string>(type: "TEXT", nullable: false),
                    ValorAnterior = table.Column<string>(type: "TEXT", nullable: true),
                    ValorNovo = table.Column<string>(type: "TEXT", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: true),
                    MetodoHttp = table.Column<string>(type: "TEXT", nullable: true),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: true),
                    CorrelationId = table.Column<string>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionalidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false),
                    Icone = table.Column<string>(type: "TEXT", nullable: true),
                    Rota = table.Column<string>(type: "TEXT", nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPadrao = table.Column<bool>(type: "INTEGER", nullable: false),
                    AplicacaoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionalidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionalidades_Aplicacoes_AplicacaoId",
                        column: x => x.AplicacaoId,
                        principalTable: "Aplicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaAplicacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AplicacaoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataAtivacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataExpiracao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LimiteUsuarios = table.Column<int>(type: "INTEGER", nullable: false),
                    ConfiguracoesJSON = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaAplicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpresaAplicacoes_Aplicacoes_AplicacaoId",
                        column: x => x.AplicacaoId,
                        principalTable: "Aplicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpresaAplicacoes_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Idioma = table.Column<string>(type: "TEXT", nullable: false),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cep = table.Column<string>(type: "TEXT", nullable: false),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Complemento = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Pais = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filiais_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    IsPadrao = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perfis_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Chave = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    IsSobrescreverGlobal = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantConfigs_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Idioma = table.Column<string>(type: "TEXT", nullable: false),
                    AvatarUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "INTEGER", nullable: false),
                    MfaHabilitado = table.Column<bool>(type: "INTEGER", nullable: false),
                    MfaSecretKey = table.Column<string>(type: "TEXT", nullable: true),
                    UltimoAcesso = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TentativasFalhasLogin = table.Column<int>(type: "INTEGER", nullable: false),
                    BloqueadoAte = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsSsoUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    SsoProvider = table.Column<string>(type: "TEXT", nullable: true),
                    SsoExternalId = table.Column<string>(type: "TEXT", nullable: true),
                    FilialId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Filiais_FilialId",
                        column: x => x.FilialId,
                        principalTable: "Filiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PerfilFuncionalidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PerfilId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FuncionalidadeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PermiteCriar = table.Column<bool>(type: "INTEGER", nullable: false),
                    PermiteLer = table.Column<bool>(type: "INTEGER", nullable: false),
                    PermiteAtualizar = table.Column<bool>(type: "INTEGER", nullable: false),
                    PermiteDeletar = table.Column<bool>(type: "INTEGER", nullable: false),
                    PermiteExecutar = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilFuncionalidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilFuncionalidades_Funcionalidades_FuncionalidadeId",
                        column: x => x.FuncionalidadeId,
                        principalTable: "Funcionalidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilFuncionalidades_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Mensagem = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    Lida = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataLeitura = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Icone = table.Column<string>(type: "TEXT", nullable: true),
                    Categoria = table.Column<string>(type: "TEXT", nullable: true),
                    EnviarPush = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnviarEmail = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataEnvioEmail = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataEnvioPush = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessoesUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TokenJti = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataUltimaAtividade = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: true),
                    IsValida = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataInvalidacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MotivoInvalidacao = table.Column<string>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessoesUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessoesUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPerfilAplicacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PerfilId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AplicacaoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsPrincipal = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataAtribuicao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataExpiracao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtribuidoPor = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPerfilAplicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioPerfilAplicacoes_Aplicacoes_AplicacaoId",
                        column: x => x.AplicacaoId,
                        principalTable: "Aplicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPerfilAplicacoes_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPerfilAplicacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacoes_Codigo",
                table: "Aplicacoes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesSistema_Chave",
                table: "ConfiguracoesSistema",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaAplicacoes_AplicacaoId",
                table: "EmpresaAplicacoes",
                column: "AplicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaAplicacoes_EmpresaId_AplicacaoId",
                table: "EmpresaAplicacoes",
                columns: new[] { "EmpresaId", "AplicacaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Cnpj",
                table: "Empresas",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Email",
                table: "Empresas",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nome",
                table: "Empresas",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_TenantId_Ativa",
                table: "Empresas",
                columns: new[] { "TenantId", "Ativa" });

            migrationBuilder.CreateIndex(
                name: "IX_Filiais_EmpresaId_Codigo",
                table: "Filiais",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Filiais_TenantId",
                table: "Filiais",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionalidades_AplicacaoId_Ativa",
                table: "Funcionalidades",
                columns: new[] { "AplicacaoId", "Ativa" });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionalidades_Codigo",
                table: "Funcionalidades",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Acao",
                table: "LogsAuditoria",
                column: "Acao");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_CorrelationId",
                table: "LogsAuditoria",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_CreatedAt",
                table: "LogsAuditoria",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_TenantId_Entidade_EntidadeId",
                table: "LogsAuditoria",
                columns: new[] { "TenantId", "Entidade", "EntidadeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_CreatedAt",
                table: "Notificacoes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UsuarioId_Lida",
                table: "Notificacoes",
                columns: new[] { "UsuarioId", "Lida" });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilFuncionalidades_FuncionalidadeId",
                table: "PerfilFuncionalidades",
                column: "FuncionalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilFuncionalidades_PerfilId_FuncionalidadeId",
                table: "PerfilFuncionalidades",
                columns: new[] { "PerfilId", "FuncionalidadeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_EmpresaId_Nome",
                table: "Perfis",
                columns: new[] { "EmpresaId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_DataExpiracao",
                table: "SessoesUsuarios",
                column: "DataExpiracao");

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_RefreshToken",
                table: "SessoesUsuarios",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_TokenJti",
                table: "SessoesUsuarios",
                column: "TokenJti",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_UsuarioId_IsValida",
                table: "SessoesUsuarios",
                columns: new[] { "UsuarioId", "IsValida" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantConfigs_EmpresaId_Chave",
                table: "TenantConfigs",
                columns: new[] { "EmpresaId", "Chave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPerfilAplicacoes_AplicacaoId",
                table: "UsuarioPerfilAplicacoes",
                column: "AplicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPerfilAplicacoes_PerfilId",
                table: "UsuarioPerfilAplicacoes",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPerfilAplicacoes_UsuarioId_AplicacaoId",
                table: "UsuarioPerfilAplicacoes",
                columns: new[] { "UsuarioId", "AplicacaoId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPerfilAplicacoes_UsuarioId_PerfilId_AplicacaoId",
                table: "UsuarioPerfilAplicacoes",
                columns: new[] { "UsuarioId", "PerfilId", "AplicacaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cpf",
                table: "Usuarios",
                column: "Cpf",
                unique: true,
                filter: "[Cpf] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FilialId",
                table: "Usuarios",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TenantId_Ativo",
                table: "Usuarios",
                columns: new[] { "TenantId", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesSistema");

            migrationBuilder.DropTable(
                name: "EmpresaAplicacoes");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "PerfilFuncionalidades");

            migrationBuilder.DropTable(
                name: "SessoesUsuarios");

            migrationBuilder.DropTable(
                name: "TenantConfigs");

            migrationBuilder.DropTable(
                name: "UsuarioPerfilAplicacoes");

            migrationBuilder.DropTable(
                name: "Funcionalidades");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Aplicacoes");

            migrationBuilder.DropTable(
                name: "Filiais");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}

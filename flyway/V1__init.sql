-- Consolidated Flyway baseline generated from EF migration
-- Contains schema created by EF Core migrations
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Aplicacoes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Aplicacoes" PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "Descricao" TEXT NOT NULL,
    "Codigo" TEXT NOT NULL,
    "Url" TEXT NOT NULL,
    "Icone" TEXT NULL,
    "Cor" TEXT NULL,
    "Ativa" INTEGER NOT NULL,
    "IsPublica" INTEGER NOT NULL,
    "SsoClientId" TEXT NULL,
    "SsoClientSecret" TEXT NULL,
    "SsoAuthority" TEXT NULL,
    "SsoScopes" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL
);

CREATE TABLE "ConfiguracoesSistema" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ConfiguracoesSistema" PRIMARY KEY,
    "Chave" TEXT NOT NULL,
    "Valor" TEXT NOT NULL,
    "Tipo" TEXT NOT NULL,
    "Descricao" TEXT NOT NULL,
    "IsPublico" INTEGER NOT NULL,
    "Categoria" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL
);

CREATE TABLE "Empresas" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Empresas" PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "NomeFantasia" TEXT NOT NULL,
    "Cnpj" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Telefone" TEXT NOT NULL,
    "LogoUrl" TEXT NOT NULL,
    "IdiomaPadrao" TEXT NOT NULL,
    "Ativa" INTEGER NOT NULL,
    "Cep" TEXT NOT NULL,
    "Rua" TEXT NOT NULL,
    "Numero" TEXT NOT NULL,
    "Complemento" TEXT NOT NULL,
    "Bairro" TEXT NOT NULL,
    "Cidade" TEXT NOT NULL,
    "Estado" TEXT NOT NULL,
    "Pais" TEXT NOT NULL DEFAULT 'BR',
    "ConfiguracoesJSON" TEXT NOT NULL DEFAULT '{}',
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL
);

CREATE TABLE "LogsAuditoria" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_LogsAuditoria" PRIMARY KEY,
    "Acao" TEXT NOT NULL,
    "Entidade" TEXT NOT NULL,
    "EntidadeId" TEXT NOT NULL,
    "ValorAnterior" TEXT NULL,
    "ValorNovo" TEXT NULL,
    "IpAddress" TEXT NOT NULL,
    "UserAgent" TEXT NOT NULL,
    "Endpoint" TEXT NULL,
    "MetodoHttp" TEXT NULL,
    "StatusCode" INTEGER NULL,
    "CorrelationId" TEXT NULL,
    "Observacao" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL
);

CREATE TABLE "Funcionalidades" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Funcionalidades" PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "Descricao" TEXT NOT NULL,
    "Codigo" TEXT NOT NULL,
    "Categoria" TEXT NOT NULL,
    "Icone" TEXT NULL,
    "Rota" TEXT NULL,
    "Ordem" INTEGER NOT NULL,
    "Ativa" INTEGER NOT NULL,
    "IsPadrao" INTEGER NOT NULL,
    "AplicacaoId" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_Funcionalidades_Aplicacoes_AplicacaoId" FOREIGN KEY ("AplicacaoId") REFERENCES "Aplicacoes" ("Id") ON DELETE SET NULL
);

CREATE TABLE "EmpresaAplicacoes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_EmpresaAplicacoes" PRIMARY KEY,
    "EmpresaId" TEXT NOT NULL,
    "AplicacaoId" TEXT NOT NULL,
    "Ativa" INTEGER NOT NULL,
    "DataAtivacao" TEXT NULL,
    "DataExpiracao" TEXT NULL,
    "LimiteUsuarios" INTEGER NOT NULL,
    "ConfiguracoesJSON" TEXT NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_EmpresaAplicacoes_Aplicacoes_AplicacaoId" FOREIGN KEY ("AplicacaoId") REFERENCES "Aplicacoes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EmpresaAplicacoes_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Filiais" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Filiais" PRIMARY KEY,
    "EmpresaId" TEXT NOT NULL,
    "Nome" TEXT NOT NULL,
    "Codigo" TEXT NOT NULL,
    "Idioma" TEXT NOT NULL,
    "Ativa" INTEGER NOT NULL,
    "Cep" TEXT NOT NULL,
    "Rua" TEXT NOT NULL,
    "Numero" TEXT NOT NULL,
    "Complemento" TEXT NOT NULL,
    "Bairro" TEXT NOT NULL,
    "Cidade" TEXT NOT NULL,
    "Estado" TEXT NOT NULL,
    "Pais" TEXT NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_Filiais_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Perfis" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Perfis" PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "Descricao" TEXT NOT NULL,
    "IsPadrao" INTEGER NOT NULL,
    "Ativo" INTEGER NOT NULL,
    "EmpresaId" TEXT NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_Perfis_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TenantConfigs" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_TenantConfigs" PRIMARY KEY,
    "EmpresaId" TEXT NOT NULL,
    "Chave" TEXT NOT NULL,
    "Valor" TEXT NOT NULL,
    "Tipo" TEXT NOT NULL,
    "Descricao" TEXT NOT NULL,
    "IsSobrescreverGlobal" INTEGER NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_TenantConfigs_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Usuarios" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Usuarios" PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "SenhaHash" TEXT NOT NULL,
    "EmpresaId" TEXT NULL,
    "Telefone" TEXT NULL,
    "Cpf" TEXT NULL,
    "Idioma" TEXT NOT NULL,
    "AvatarUrl" TEXT NULL,
    "Ativo" INTEGER NOT NULL,
    "EmailConfirmado" INTEGER NOT NULL,
    "MfaHabilitado" INTEGER NOT NULL,
    "MfaSecretKey" TEXT NULL,
    "UltimoAcesso" TEXT NULL,
    "TentativasFalhasLogin" INTEGER NOT NULL,
    "BloqueadoAte" TEXT NULL,
    "IsSsoUser" INTEGER NOT NULL,
    "SsoProvider" TEXT NULL,
    "SsoExternalId" TEXT NULL,
    "FilialId" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_Usuarios_Empresas_TenantId" FOREIGN KEY ("TenantId") REFERENCES "Empresas" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Usuarios_Filiais_FilialId" FOREIGN KEY ("FilialId") REFERENCES "Filiais" ("Id") ON DELETE SET NULL
);

CREATE TABLE "PerfilFuncionalidades" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_PerfilFuncionalidades" PRIMARY KEY,
    "PerfilId" TEXT NOT NULL,
    "FuncionalidadeId" TEXT NOT NULL,
    "PermiteCriar" INTEGER NOT NULL,
    "PermiteLer" INTEGER NOT NULL,
    "PermiteAtualizar" INTEGER NOT NULL,
    "PermiteDeletar" INTEGER NOT NULL,
    "PermiteExecutar" INTEGER NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_PerfilFuncionalidades_Funcionalidades_FuncionalidadeId" FOREIGN KEY ("FuncionalidadeId") REFERENCES "Funcionalidades" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PerfilFuncionalidades_Perfis_PerfilId" FOREIGN KEY ("PerfilId") REFERENCES "Perfis" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Notificacoes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Notificacoes" PRIMARY KEY,
    "UsuarioId" TEXT NOT NULL,
    "Titulo" TEXT NOT NULL,
    "Mensagem" TEXT NOT NULL,
    "Tipo" TEXT NOT NULL,
    "Link" TEXT NULL,
    "Lida" INTEGER NOT NULL,
    "DataLeitura" TEXT NULL,
    "Icone" TEXT NULL,
    "Categoria" TEXT NULL,
    "EnviarPush" INTEGER NOT NULL,
    "EnviarEmail" INTEGER NOT NULL,
    "DataEnvioEmail" TEXT NULL,
    "DataEnvioPush" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_Notificacoes_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
);

CREATE TABLE "SessoesUsuarios" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_SessoesUsuarios" PRIMARY KEY,
    "UsuarioId" TEXT NOT NULL,
    "TokenJti" TEXT NOT NULL,
    "RefreshToken" TEXT NOT NULL,
    "DataCriacao" TEXT NOT NULL,
    "DataExpiracao" TEXT NOT NULL,
    "DataUltimaAtividade" TEXT NULL,
    "IpAddress" TEXT NOT NULL,
    "UserAgent" TEXT NOT NULL,
    "DeviceId" TEXT NULL,
    "IsValida" INTEGER NOT NULL,
    "DataInvalidacao" TEXT NULL,
    "MotivoInvalidacao" TEXT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_SessoesUsuarios_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UsuarioPerfilAplicacoes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_UsuarioPerfilAplicacoes" PRIMARY KEY,
    "UsuarioId" TEXT NOT NULL,
    "PerfilId" TEXT NOT NULL,
    "AplicacaoId" TEXT NOT NULL,
    "IsPrincipal" INTEGER NOT NULL,
    "DataAtribuicao" TEXT NULL,
    "DataExpiracao" TEXT NULL,
    "AtribuidoPor" TEXT NOT NULL,
    "TenantId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "CreatedBy" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    "DeletedAt" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "RowVersion" BLOB NOT NULL,
    CONSTRAINT "FK_UsuarioPerfilAplicacoes_Aplicacoes_AplicacaoId" FOREIGN KEY ("AplicacaoId") REFERENCES "Aplicacoes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UsuarioPerfilAplicacoes_Perfis_PerfilId" FOREIGN KEY ("PerfilId") REFERENCES "Perfis" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UsuarioPerfilAplicacoes_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Aplicacoes_Codigo" ON "Aplicacoes" ("Codigo");

CREATE UNIQUE INDEX "IX_ConfiguracoesSistema_Chave" ON "ConfiguracoesSistema" ("Chave");

CREATE INDEX "IX_EmpresaAplicacoes_AplicacaoId" ON "EmpresaAplicacoes" ("AplicacaoId");

CREATE UNIQUE INDEX "IX_EmpresaAplicacoes_EmpresaId_AplicacaoId" ON "EmpresaAplicacoes" ("EmpresaId", "AplicacaoId");

CREATE UNIQUE INDEX "IX_Empresas_Cnpj" ON "Empresas" ("Cnpj");

CREATE INDEX "IX_Empresas_Email" ON "Empresas" ("Email");

CREATE INDEX "IX_Empresas_Nome" ON "Empresas" ("Nome");

CREATE INDEX "IX_Empresas_TenantId_Ativa" ON "Empresas" ("TenantId", "Ativa");

CREATE UNIQUE INDEX "IX_Filiais_EmpresaId_Codigo" ON "Filiais" ("EmpresaId", "Codigo");

CREATE INDEX "IX_Filiais_TenantId" ON "Filiais" ("TenantId");

CREATE INDEX "IX_Funcionalidades_AplicacaoId_Ativa" ON "Funcionalidades" ("AplicacaoId", "Ativa");

CREATE UNIQUE INDEX "IX_Funcionalidades_Codigo" ON "Funcionalidades" ("Codigo");

CREATE INDEX "IX_LogsAuditoria_Acao" ON "LogsAuditoria" ("Acao");

CREATE INDEX "IX_LogsAuditoria_CorrelationId" ON "LogsAuditoria" ("CorrelationId");

CREATE INDEX "IX_LogsAuditoria_CreatedAt" ON "LogsAuditoria" ("CreatedAt");

CREATE INDEX "IX_LogsAuditoria_TenantId_Entidade_EntidadeId" ON "LogsAuditoria" ("TenantId", "Entidade", "EntidadeId");

CREATE INDEX "IX_Notificacoes_CreatedAt" ON "Notificacoes" ("CreatedAt");

CREATE INDEX "IX_Notificacoes_UsuarioId_Lida" ON "Notificacoes" ("UsuarioId", "Lida");

CREATE INDEX "IX_PerfilFuncionalidades_FuncionalidadeId" ON "PerfilFuncionalidades" ("FuncionalidadeId");

CREATE UNIQUE INDEX "IX_PerfilFuncionalidades_PerfilId_FuncionalidadeId" ON "PerfilFuncionalidades" ("PerfilId", "FuncionalidadeId");

CREATE UNIQUE INDEX "IX_Perfis_EmpresaId_Nome" ON "Perfis" ("EmpresaId", "Nome");

CREATE INDEX "IX_SessoesUsuarios_DataExpiracao" ON "SessoesUsuarios" ("DataExpiracao");

CREATE UNIQUE INDEX "IX_SessoesUsuarios_RefreshToken" ON "SessoesUsuarios" ("RefreshToken");

CREATE UNIQUE INDEX "IX_SessoesUsuarios_TokenJti" ON "SessoesUsuarios" ("TokenJti");

CREATE INDEX "IX_SessoesUsuarios_UsuarioId_IsValida" ON "SessoesUsuarios" ("UsuarioId", "IsValida");

CREATE UNIQUE INDEX "IX_TenantConfigs_EmpresaId_Chave" ON "TenantConfigs" ("EmpresaId", "Chave");

CREATE INDEX "IX_UsuarioPerfilAplicacoes_AplicacaoId" ON "UsuarioPerfilAplicacoes" ("AplicacaoId");

CREATE INDEX "IX_UsuarioPerfilAplicacoes_PerfilId" ON "UsuarioPerfilAplicacoes" ("PerfilId");

CREATE INDEX "IX_UsuarioPerfilAplicacoes_UsuarioId_AplicacaoId" ON "UsuarioPerfilAplicacoes" ("UsuarioId", "AplicacaoId");

CREATE UNIQUE INDEX "IX_UsuarioPerfilAplicacoes_UsuarioId_PerfilId_AplicacaoId" ON "UsuarioPerfilAplicacoes" ("UsuarioId", "PerfilId", "AplicacaoId");

CREATE UNIQUE INDEX "IX_Usuarios_Cpf" ON "Usuarios" ("Cpf") WHERE [Cpf] IS NOT NULL;

CREATE INDEX "IX_Usuarios_Email" ON "Usuarios" ("Email");

CREATE INDEX "IX_Usuarios_FilialId" ON "Usuarios" ("FilialId");

CREATE INDEX "IX_Usuarios_TenantId_Ativo" ON "Usuarios" ("TenantId", "Ativo");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260611114500_Initial_20260611074442', '8.0.0');

COMMIT;

BEGIN TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260611114749_Initial_20260611074740', '8.0.0');

COMMIT;

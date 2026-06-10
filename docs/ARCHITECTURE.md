# AccessControlSaaS - Arquitetura

## Visao Geral

AccessControlSaaS e uma plataforma enterprise de controle de acesso multi-tenant, multi-idioma, com arquitetura moderna, alta performance, seguranca enterprise-grade e pipeline CI/CD automatizado.

## Arquitetura - .NET 10 Clean Architecture (4 Camadas)

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                        │
│  ASP.NET Core 10 Web API + React 19 + Vite + TypeScript     │
│  PWA (Progressive Web App) para mobile                      │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    APPLICATION LAYER                         │
│  CQRS (MediatR), DTOs, Validators (FluentValidation),       │
│  Mappers (AutoMapper), Behaviors (Pipeline)                  │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                              │
│  Entities, Value Objects, Domain Events, Specifications,    │
│  Interfaces de Repositorio                                   │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    INFRASTRUCTURE LAYER                      │
│  EF Core 10, SQL Server 2022, Identity, JWT, OAuth2,         │
│  Redis Cache, RabbitMQ/MassTransit, Serilog, OpenTelemetry   │
└─────────────────────────────────────────────────────────────┘
```

## Principios Arquiteturais

- SOLID em todas as camadas
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple, Stupid)
- YAGNI (You Aren't Gonna Need It)
- Dependency Inversion - dependencias apontam para dentro
- CQRS separando comandos de queries
- Repository Pattern com Unit of Work
- Domain-Driven Design (DDD) para regras de negocio complexas
- Vertical Slice Architecture por feature

## Modelo de Dados

### Entidades Principais

| Entidade | Descricao |
|----------|-----------|
| Empresa | Tenant principal, define idioma default, logo, endereco |
| Filial | Sub-tenant com idioma proprio, endereco |
| Usuario | Usuario com idioma proprio, perfis por aplicacao |
| Perfil | Perfil de acesso (Admin, Gestor, Operador, etc) |
| Funcionalidade | Funcionalidades do sistema (CRUD granular) |
| PerfilFuncionalidade | Ligacao N:N Perfil <-> Funcionalidade |
| Aplicacao | Aplicacoes/Sistemas gerenciados |
| EmpresaAplicacao | Ligacao N:N Empresa <-> Aplicacao |
| ConfiguracaoSistema | Configuracoes globais do tenant |
| LogAuditoria | Trail de auditoria completo |
| SessaoUsuario | Sessoes ativas para controle de seguranca |
| Notificacao | Notificacoes in-app e push |
| TenantConfig | Configuracoes especificas de tenant |

## Regras de Dados

- Multi-tenancy: Row-Level Security (RLS) do SQL Server + TenantId em todas as tabelas
- Endereco: Formato adaptativo por pais (CEP/rua/numero/cidade/estado/pais)
- Logo/Marca: Upload com validacao de tipo/tamanho, armazenamento em blob
- Soft Delete: Todas as entidades tem DeletedAt e IsDeleted
- Auditoria: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy em todas as entidades
- Versionamento: RowVersion (timestamp) para concorrencia otimista
- Indices: Indices compostos em TenantId + campos de busca frequentes
- Particionamento: Particionar tabelas de log por data (mensal)
- JSON Columns: Usar JSON para configuracoes flexiveis por tenant
- Full-Text Search: Habilitar em campos de busca (nome, email, descricao)
- Always Encrypted: Para dados sensiveis (CPF, documentos)

## Seguranca

### Autenticacao & Autorizacao
- JWT Bearer Tokens com RS256 (chaves assimetricas)
- Refresh Tokens rotacionaveis, armazenados em HttpOnly cookies
- SSO OAuth2/OIDC (Google, Microsoft, Azure AD, Okta)
- Claims-based Authorization com perfil por aplicacao
- Tela de login aparece apenas quando usuario NAO vem por SSO
- Multi-Factor Authentication (MFA) via TOTP/SMS/Email
- Password Policy: Minimo 12 caracteres, complexidade, historico
- Account Lockout: 5 tentativas falhas = 15 minutos bloqueio
- Session Management: Limite de sessoes simultaneas, invalidacao remota

### Protecao de Dados
- HTTPS Only (HSTS, TLS 1.3)
- CSP (Content Security Policy) estrito
- XSS Protection: Output encoding, CSP, anti-forgery tokens
- CSRF Protection: Double-submit cookie pattern
- SQL Injection: EF Core parameterized queries
- Rate Limiting: 100 req/min por IP, 1000 req/min por usuario
- Input Validation: FluentValidation + Data Annotations + sanitizacao
- File Upload Security: Magic number validation, size limits, AV scanning
- Sensitive Data Logging: Nunca logar senhas, tokens, dados pessoais
- Data Masking: Mascarar dados sensiveis em logs e respostas API

## Multi-Idioma & Multi-Tenant

### Hierarquia de Idiomas
1. Usuario (maior prioridade)
2. Filial
3. Empresa (default)
4. Sistema (fallback: PT)

### Idiomas Suportados
- PT - Portugues (Brasil) - default
- EN - English (US)
- ES - Espanol

## Frontend

### Stack Tecnologico
- React 19 (RC ou stable)
- Vite 6 (build tool)
- TypeScript 5.5
- Tailwind CSS 3.4 (utility-first CSS)
- Radix UI (headless components acessiveis)
- TanStack Query 5 (data fetching)
- Zustand 4 (state management)
- React Router 6 (routing)
- React Hook Form 7 (forms)
- Zod 3 (schema validation)
- Framer Motion (animations)
- Lucide React (icons)

### Design System - Dark Theme Profissional
- Cores: Slate/Zinc base, Indigo/Blue primary, Emerald success, Rose error, Amber warning
- Tipografia: Inter (Google Fonts), scale modular
- Espacamento: 4px base grid system
- Bordas: Radius consistente (sm, md, lg, xl, full)
- Shadows: Escala de elevacao (shadow-sm ate shadow-2xl)
- Modo escuro por padrao com toggle para claro

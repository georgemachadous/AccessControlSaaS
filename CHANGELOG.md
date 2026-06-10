# Changelog

## [1.0.0] - 2026-06-03

### Adicionado
- Projeto completo backend .NET 10 com Clean Architecture
- 9 entidades: Company, Branch, Application, Functionality, Role, RoleFunctionality, User, UserApplicationRole, Permission, AuditLog
- Autenticacao JWT com claims por aplicacao
- SSO OAuth2 support (Google, Microsoft, etc.)
- Seed data com Super Admin padrao
- Frontend React 19 + Vite + TypeScript
- Dashboard com resumo e atividades recentes
- CRUD universal para todas as entidades
- SQLite como banco de dados

### Corrigido
- Referencia circular entre Application e Infrastructure
- Erro CS0117 no Swagger (OpenApiSecurityScheme.Reference)
- Strings de escape quebradas no Program.cs
- Namespaces inconsistentes
- Duplicacao de entidades e enums

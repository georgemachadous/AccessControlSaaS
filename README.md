# AccessControl SaaS

Sistema de controle de acesso multi-tenant com autenticacao JWT, SSO OAuth2 e gestao de permissoes por aplicacao.

## Estrutura

```
AccessControlSaaS/
├── backend/          .NET 10 Web API + EF Core SQLite
│   ├── AccessControlSaaS.sln
│   └── src/
│       ├── AccessControl.Domain/       (Entidades + Enums)
│       ├── AccessControl.Application/  (DTOs)
│       ├── AccessControl.Infrastructure/ (DbContext + Seed)
│       └── AccessControl.API/          (Services + Controllers + Program)
├── frontend/         React 19 + Vite + TypeScript
└── start-all.ps1     Script para iniciar tudo
```

## Requisitos

- .NET 10 SDK
- Node.js 20+
- PowerShell (Windows) ou Bash (Linux/Mac)

## Como executar

### Backend
```bash
cd backend
dotnet restore AccessControlSaaS.sln
dotnet build AccessControlSaaS.sln
dotnet run --project src/AccessControl.API
```
Backend roda em: https://localhost:5000

### Frontend
```bash
cd frontend
npm install
npm run dev
```
Frontend roda em: http://localhost:5173

## Credenciais Padrao

- Email: superadmin@admin.com
- Senha: SuperAdmin@123

## Endpoints da API

| Recurso | Endpoint |
|---------|----------|
| Auth | POST /api/auth/login |
| Auth SSO | POST /api/auth/sso |
| Companies | GET/POST /api/companies |
| Branches | GET/POST /api/branches |
| Applications | GET/POST /api/applications |
| Functionalities | GET/POST /api/functionalities |
| Roles | GET/POST /api/roles |
| RoleFunctionalities | GET/POST /api/rolefunctionalities |
| Users | GET/POST /api/users |
| UserAppRoles | GET/POST /api/userapplicationroles |
| Permissions | GET/POST /api/permissions |
| Dashboard | GET /api/dashboard/summary |

## Modelo de Dados

- **Company** -> Branches, Applications, Users
- **Application** -> Functionalities, Roles, UserApplicationRoles
- **Role** -> RoleFunctionalities
- **User** -> UserApplicationRoles, Permissions
- **RoleFunctionality** -> Permissoes CRUD por funcionalidade

## JWT Claims

O token JWT contem claims por aplicacao no formato:
```
app_{applicationId}: { applicationId, applicationName, roleId, roleName, permissions[] }
```

## Licenca

MIT

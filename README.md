# AccessControlSaaS

Enterprise Access Control Platform - Multi-tenant, Multi-idioma, com arquitetura moderna e seguranca enterprise-grade.

## Arquitetura

- **Backend**: .NET 10 Clean Architecture (4 camadas)
- **Frontend**: React 19 + Vite + TypeScript + Tailwind CSS
- **Database**: SQL Server 2022 com Row-Level Security
- **Cache**: Redis
- **Observability**: OpenTelemetry + Prometheus + Grafana + Jaeger
- **CI/CD**: GitHub Actions com Blue-Green Deploy

## Estrutura

```
AccessControlSaaS/
├── src/
│   ├── Api/              # ASP.NET Core 10 Web API
│   ├── Application/      # CQRS, DTOs, Validators
│   ├── Domain/           # Entities, Value Objects, Events
│   ├── Infrastructure/   # EF Core, Identity, Services
│   └── Frontend/         # React 19 + Vite + TypeScript
├── tests/
│   ├── Unit/             # xUnit + NSubstitute
│   ├── Integration/      # WebApplicationFactory
│   ├── E2E/              # Playwright
│   └── Performance/      # k6
├── docker/               # Dockerfiles & Compose
├── scripts/              # Deploy & Backup scripts
├── monitoring/           # Prometheus & Grafana
├── nginx/                # Nginx config
└── docs/                 # Documentation
```

## Quick Start

### Backend
```bash
dotnet restore
dotnet build
dotnet run --project src/Api
```

### Frontend
```bash
cd src/Frontend
npm install
npm run dev
```

### Docker
```bash
docker compose -f docker/docker-compose.yml up -d
```

## Documentacao

- [Arquitetura](docs/ARCHITECTURE.md)
- [API](docs/API.md)
- [Deploy](docs/DEPLOY.md)
- [Seguranca](docs/SECURITY.md)

## Licenca

MIT

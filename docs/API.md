# AccessControlSaaS - API Documentation

## Base URL
```
https://api.accesscontrol.com/api
```

## Authentication
All endpoints (except login/refresh) require Bearer token:
```
Authorization: Bearer <token>
```

## Endpoints

### Authentication
- `POST /api/auth/login` - Login with email/password
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - Logout current session
- `POST /api/auth/change-password` - Change password
- `POST /api/auth/recover-password` - Request password recovery
- `POST /api/auth/reset-password` - Reset password with token
- `GET /api/auth/sso/{provider}` - SSO login (Google, Microsoft)
- `GET /api/auth/me` - Get current user
- `POST /api/auth/mfa/setup` - Setup MFA
- `POST /api/auth/mfa/validate` - Validate MFA code

### Empresas
- `GET /api/empresas` - List all empresas (paginated)
- `GET /api/empresas/{id}` - Get empresa by ID
- `POST /api/empresas` - Create new empresa
- `PUT /api/empresas/{id}` - Update empresa
- `DELETE /api/empresas/{id}` - Delete empresa (soft delete)
- `GET /api/empresas/{id}/stats` - Get empresa statistics
- `POST /api/empresas/{id}/logo` - Upload logo

### Usuarios
- `GET /api/usuarios` - List all usuarios (paginated)
- `GET /api/usuarios/{id}` - Get usuario by ID
- `POST /api/usuarios` - Create new usuario
- `PUT /api/usuarios/{id}` - Update usuario
- `DELETE /api/usuarios/{id}` - Delete usuario (soft delete)
- `POST /api/usuarios/{id}/ativar` - Activate usuario
- `POST /api/usuarios/{id}/desativar` - Deactivate usuario
- `POST /api/usuarios/{id}/avatar` - Upload avatar
- `GET /api/usuarios/{id}/perfis` - Get usuario profiles
- `POST /api/usuarios/{id}/perfis` - Assign profile to usuario

### Perfis
- `GET /api/perfis` - List all perfis (paginated)
- `GET /api/perfis/{id}` - Get perfil by ID
- `POST /api/perfis` - Create new perfil
- `PUT /api/perfis/{id}` - Update perfil
- `DELETE /api/perfis/{id}` - Delete perfil (soft delete)
- `GET /api/perfis/{id}/funcionalidades` - Get perfil funcionalidades
- `POST /api/perfis/{id}/funcionalidades` - Assign funcionalidade to perfil
- `DELETE /api/perfis/{id}/funcionalidades/{funcId}` - Remove funcionalidade from perfil

### Aplicacoes
- `GET /api/aplicacoes` - List all aplicacoes (paginated)
- `GET /api/aplicacoes/{id}` - Get aplicacao by ID
- `POST /api/aplicacoes` - Create new aplicacao
- `PUT /api/aplicacoes/{id}` - Update aplicacao
- `DELETE /api/aplicacoes/{id}` - Delete aplicacao (soft delete)
- `GET /api/aplicacoes/{id}/usuarios` - Get usuarios for aplicacao

### Dashboard
- `GET /api/dashboard/stats` - Get dashboard statistics
- `GET /api/dashboard/logins-por-dia` - Get logins per day

### Auditoria
- `GET /api/auditoria` - List audit logs (paginated)

### Notificacoes
- `GET /api/notificacoes` - List notifications
- `GET /api/notificacoes/nao-lidas/count` - Get unread count
- `POST /api/notificacoes/{id}/ler` - Mark as read

## Health Checks
- `GET /health` - Liveness probe
- `GET /health/ready` - Readiness probe
- `GET /metrics` - Prometheus metrics

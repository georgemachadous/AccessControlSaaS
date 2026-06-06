# AccessControlSaaS - Deploy Guide

## Requisitos da VM
- OS: Ubuntu 24.04 LTS
- CPU: 4 vCPUs
- RAM: 16 GB
- Disco: 100 GB SSD
- Rede: VCN/VPC com subnets publicas e privadas

## Instalacao Automatica

```bash
# 1. Clone o repositorio
git clone https://github.com/seu-repo/AccessControlSaaS.git
cd AccessControlSaaS

# 2. Execute o script de instalacao
chmod +x scripts/install.sh
./scripts/install.sh

# 3. Configure as variaveis de ambiente
sudo nano /opt/accesscontrol/.env

# 4. Inicie os servicos
sudo systemctl start accesscontrol-api
```

## Configuracao .env
```
SQL_SA_PASSWORD=YourStrong@Passw0rd
JWT_ISSUER=AccessControlSaaS
JWT_AUDIENCE=AccessControlSaaS
JWT_PRIVATE_KEY=-----BEGIN RSA PRIVATE KEY-----
...
-----END RSA PRIVATE KEY-----
JWT_PUBLIC_KEY=-----BEGIN PUBLIC KEY-----
...
-----END PUBLIC KEY-----
GRAFANA_PASSWORD=admin123
```

## SSL Let's Encrypt
```bash
sudo certbot --nginx -d app.accesscontrol.com --non-interactive --agree-tos --email admin@accesscontrol.com
sudo certbot renew --dry-run
```

## Backup Automatico
```bash
# Adicione ao crontab
0 2 * * * /opt/accesscontrol/scripts/backup.sh >> /var/log/accesscontrol-backup.log 2>&1
```

## Deploy Manual
```bash
# Pull latest images
docker compose -f /opt/accesscontrol/docker-compose.yml pull

# Deploy with zero downtime
docker compose -f /opt/accesscontrol/docker-compose.yml up -d --no-deps --scale api=2

# Health check
sleep 30
curl -f http://localhost:8080/health/ready || exit 1

# Scale down old
docker compose -f /opt/accesscontrol/docker-compose.yml up -d --scale api=1
```

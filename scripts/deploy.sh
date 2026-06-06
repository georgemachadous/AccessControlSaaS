#!/bin/bash
set -euo pipefail

echo "=== AccessControlSaaS Deploy Script ==="

# Pull latest images
echo "Pulling latest images..."
docker compose -f /opt/accesscontrol/docker-compose.yml pull

# Deploy with zero downtime
echo "Deploying..."
docker compose -f /opt/accesscontrol/docker-compose.yml up -d --no-deps --scale api=2

# Health check
echo "Running health check..."
sleep 30
curl -f http://localhost:8080/health/ready || exit 1

# Scale down old containers
echo "Scaling down old containers..."
docker compose -f /opt/accesscontrol/docker-compose.yml up -d --scale api=1

echo "=== Deploy Complete ==="

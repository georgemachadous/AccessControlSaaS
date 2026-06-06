#!/bin/bash
set -euo pipefail

echo "=== AccessControlSaaS - Ubuntu 24.04 Installation Script ==="

# Update system
sudo apt-get update && sudo apt-get upgrade -y

# Install dependencies
sudo apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release \
    software-properties-common \
    nginx \
    certbot \
    python3-certbot-nginx \
    ufw \
    fail2ban \
    htop \
    git \
    unzip \
    jq

# Install Docker
echo "Installing Docker..."
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
sudo usermod -aG docker $USER

# Install .NET 10
echo "Installing .NET 10..."
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-10.0 aspnetcore-runtime-10.0

# Install Node.js 20
echo "Installing Node.js 20..."
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# Firewall
echo "Configuring firewall..."
sudo ufw default deny incoming
sudo ufw default allow outgoing
sudo ufw allow ssh
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow 8080/tcp
sudo ufw --force enable

# Fail2ban
echo "Configuring fail2ban..."
sudo cp /etc/fail2ban/jail.conf /etc/fail2ban/jail.local
sudo systemctl enable fail2ban
sudo systemctl start fail2ban

# Create directories
echo "Creating application directories..."
sudo mkdir -p /opt/accesscontrol
sudo mkdir -p /opt/accesscontrol/backups
sudo mkdir -p /opt/accesscontrol/logs
sudo mkdir -p /opt/accesscontrol/scripts
sudo chown -R $USER:$USER /opt/accesscontrol

# Systemd service for Docker Compose
echo "Creating systemd service..."
sudo tee /etc/systemd/system/accesscontrol-api.service > /dev/null <<EOF
[Unit]
Description=AccessControl SaaS API
After=network.target docker.service
Requires=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
WorkingDirectory=/opt/accesscontrol
ExecStart=/usr/bin/docker compose -f docker-compose.yml up -d
ExecStop=/usr/bin/docker compose -f docker-compose.yml down
User=$USER

[Install]
WantedBy=multi-user.target
EOF

sudo systemctl daemon-reload
sudo systemctl enable accesscontrol-api

echo "=== Installation Complete ==="
echo "Please:"
echo "1. Log out and log back in for Docker permissions"
echo "2. Copy your application files to /opt/accesscontrol"
echo "3. Configure .env file with your secrets"
echo "4. Run: sudo systemctl start accesscontrol-api"

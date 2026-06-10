#!/bin/bash

# AccessControl SaaS - Start All (Linux/Ubuntu)
# Salve como: start-all.sh
# Execute: chmod +x start-all.sh && ./start-all.sh

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BACKEND_PATH="$SCRIPT_DIR/backend"
FRONTEND_PATH="$SCRIPT_DIR/frontend"

# Cores para output
CYAN='\033[0;36m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${CYAN}Starting AccessControl SaaS...${NC}"

# Verificar se as pastas existem
if [ ! -d "$BACKEND_PATH" ]; then
    echo -e "\033[0;31mErro: Pasta backend nao encontrada em $BACKEND_PATH${NC}"
    exit 1
fi

if [ ! -d "$FRONTEND_PATH" ]; then
    echo -e "\033[0;31mErro: Pasta frontend nao encontrada em $FRONTEND_PATH${NC}"
    exit 1
fi

# Verificar se dotnet esta instalado
if ! command -v dotnet &> /dev/null; then
    echo -e "\033[0;31mErro: dotnet nao esta instalado${NC}"
    echo "Instale: https://learn.microsoft.com/en-us/dotnet/core/install/linux"
    exit 1
fi

# Verificar se npm esta instalado
if ! command -v npm &> /dev/null; then
    echo -e "\033[0;31mErro: npm nao esta instalado${NC}"
    echo "Instale: sudo apt update && sudo apt install -y nodejs npm"
    exit 1
fi

# ============================================
# START BACKEND
# ============================================
echo -e "${CYAN}Iniciando Backend...${NC}"
cd "$BACKEND_PATH" || exit 1

# Verificar se ja foi buildado
if [ ! -d "$BACKEND_PATH/src/AccessControl.API/bin" ]; then
    echo "Primeira execucao - fazendo build..."
    dotnet restore AccessControlSaaS.sln
    dotnet build AccessControlSaaS.sln --no-restore
fi

# Iniciar backend em background
nohup dotnet run --project src/AccessControl.API --no-build > "$SCRIPT_DIR/backend.log" 2>&1 &
BACKEND_PID=$!

echo "Backend PID: $BACKEND_PID"
echo "Backend log: $SCRIPT_DIR/backend.log"

# ============================================
# WAIT FOR BACKEND
# ============================================
echo "Aguardando backend iniciar..."
sleep 5

# Verificar se backend esta rodando
if ! curl -s http://localhost:5000/swagger/index.html > /dev/null 2>&1; then
    echo "Aguardando mais 5 segundos..."
    sleep 5
fi

# ============================================
# START FRONTEND
# ============================================
echo -e "${CYAN}Iniciando Frontend...${NC}"
cd "$FRONTEND_PATH" || exit 1

# Verificar se node_modules existe
if [ ! -d "$FRONTEND_PATH/node_modules" ]; then
    echo "Instalando dependencias do frontend..."
    npm install
fi

# Iniciar frontend em background
nohup npm run dev > "$SCRIPT_DIR/frontend.log" 2>&1 &
FRONTEND_PID=$!

echo "Frontend PID: $FRONTEND_PID"
echo "Frontend log: $SCRIPT_DIR/frontend.log"

# ============================================
# STATUS
# ============================================
echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}AccessControl SaaS iniciado!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${GREEN}Backend API:${NC}   http://localhost:5000"
echo -e "${GREEN}Swagger UI:${NC}   http://localhost:5000/swagger"
echo -e "${GREEN}Frontend:${NC}     http://localhost:5173"
echo ""
echo -e "${YELLOW}Credentials:${NC}  superadmin@admin.com / SuperAdmin@123"
echo ""
echo "Para parar os servicos:"
echo "  kill $BACKEND_PID $FRONTEND_PID"
echo ""
echo "Para ver logs em tempo real:"
echo "  tail -f $SCRIPT_DIR/backend.log"
echo "  tail -f $SCRIPT_DIR/frontend.log"

# Salvar PIDs para parar depois
echo "$BACKEND_PID" > "$SCRIPT_DIR/.backend.pid"
echo "$FRONTEND_PID" > "$SCRIPT_DIR/.frontend.pid"

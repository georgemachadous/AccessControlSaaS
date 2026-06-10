#!/bin/bash

# AccessControl SaaS - Stop All (Linux/Ubuntu)
# Salve como: stop-all.sh
# Execute: chmod +x stop-all.sh && ./stop-all.sh

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

RED='\033[0;31m'
GREEN='\033[0;32m'
NC='\033[0m'

echo -e "${RED}Parando AccessControl SaaS...${NC}"

# Parar pelo PID salvo
if [ -f "$SCRIPT_DIR/.backend.pid" ]; then
    BACKEND_PID=$(cat "$SCRIPT_DIR/.backend.pid")
    if kill -0 "$BACKEND_PID" 2>/dev/null; then
        kill "$BACKEND_PID"
        echo "Backend parado (PID: $BACKEND_PID)"
    fi
    rm "$SCRIPT_DIR/.backend.pid"
fi

if [ -f "$SCRIPT_DIR/.frontend.pid" ]; then
    FRONTEND_PID=$(cat "$SCRIPT_DIR/.frontend.pid")
    if kill -0 "$FRONTEND_PID" 2>/dev/null; then
        kill "$FRONTEND_PID"
        echo "Frontend parado (PID: $FRONTEND_PID)"
    fi
    rm "$SCRIPT_DIR/.frontend.pid"
fi

# Fallback: matar processos dotnet e npm run dev
pkill -f "dotnet run --project src/AccessControl.API" 2>/dev/null
pkill -f "npm run dev" 2>/dev/null

echo -e "${GREEN}Todos os servicos foram parados!${NC}"

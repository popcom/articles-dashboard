#!/bin/bash

echo "🚀 Starting Article Dashboard (Backend + Frontend)..."

# Set directories
BACKEND_DIR="./backend/ArticleDashboard.API"
FRONTEND_DIR="./frontend/article-dashboard-app"

# Start backend
echo "🔧 Launching .NET Backend..."
cd $BACKEND_DIR
dotnet run &
BACKEND_PID=$!
cd - > /dev/null

# Start frontend
echo "🎨 Launching Angular Frontend..."
cd $FRONTEND_DIR
npm start &
FRONTEND_PID=$!
cd - > /dev/null

# Output PIDs and wait
echo "✅ Backend PID: $BACKEND_PID"
echo "✅ Frontend PID: $FRONTEND_PID"
echo "🌐 Visit http://localhost:4200 for the frontend"
echo "🧪 Visit https://localhost:7023/swagger for the API"
echo ""
echo "Press [CTRL+C] to stop both processes."

# Trap CTRL+C to stop both
trap "echo '🛑 Stopping...'; kill $BACKEND_PID $FRONTEND_PID" SIGINT

wait

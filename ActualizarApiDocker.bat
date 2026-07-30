@echo off
echo ===============================
echo Actualizando SistemaVentas API
echo ===============================

cd /d "%~dp0"

docker compose up -d --build api

echo.
echo ===============================
echo API actualizada
echo Swagger:
echo http://localhost:8080/swagger/index.html
echo ===============================

pause
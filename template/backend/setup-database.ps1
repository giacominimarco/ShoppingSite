# Script para configurar o banco de dados PostgreSQL
Write-Host "Configurando banco de dados PostgreSQL..." -ForegroundColor Green

# Verificar se o Docker está rodando
Write-Host "Verificando Docker..." -ForegroundColor Yellow
try {
    docker --version | Out-Null
    Write-Host "Docker encontrado" -ForegroundColor Green
} catch {
    Write-Host "Docker não encontrado. Por favor, instale o Docker Desktop." -ForegroundColor Red
    exit 1
}

# Verificar se o Docker Desktop está rodando
try {
    docker ps | Out-Null
    Write-Host "Docker Desktop está rodando" -ForegroundColor Green
} catch {
    Write-Host "Docker Desktop não está rodando. Por favor, inicie o Docker Desktop." -ForegroundColor Red
    exit 1
}

# Parar containers existentes
Write-Host "Parando containers existentes..." -ForegroundColor Yellow
docker-compose down

# Iniciar apenas o PostgreSQL
Write-Host "Iniciando PostgreSQL..." -ForegroundColor Yellow
docker-compose up -d ambev.developerevaluation.database

# Aguardar o PostgreSQL inicializar
Write-Host "Aguardando PostgreSQL inicializar (30s)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Verificar se o PostgreSQL está rodando
Write-Host "Verificando status do PostgreSQL..." -ForegroundColor Yellow
$postgresContainer = docker ps --filter "name=ambev_developer_evaluation_database" --format "table {{.Names}}\t{{.Status}}"
Write-Host $postgresContainer

# Testar conexão via docker exec
Write-Host "Testando conexão com PostgreSQL via container..." -ForegroundColor Yellow
$maxRetries = 10
$retryDelay = 3
$success = $false

for ($i = 1; $i -le $maxRetries; $i++) {
    try {
        docker exec ambev_developer_evaluation_database psql -U developer -d developer_evaluation -c "\q" 2>$null
        $success = $true
        break
    } catch {
        Write-Host "Tentativa $i/$maxRetries falhou, aguardando $retryDelay segundos..."
        Start-Sleep -Seconds $retryDelay
    }
}

if ($success) {
    Write-Host "Conexão com PostgreSQL estabelecida via container!" -ForegroundColor Green
} else {
    Write-Host "Falha na conexão com PostgreSQL após múltiplas tentativas" -ForegroundColor Red
}

# Informações do banco
Write-Host "`nInformações do banco:" -ForegroundColor Cyan
Write-Host "   Host: localhost" -ForegroundColor White
Write-Host "   Porta: 5432" -ForegroundColor White
Write-Host "   Database: developer_evaluation" -ForegroundColor White
Write-Host "   Usuário: developer" -ForegroundColor White
Write-Host "   Senha: ev@luAt10n" -ForegroundColor White
Write-Host "   Connection string pronta: Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n" -ForegroundColor White

# Próximos passos
Write-Host "`nPróximos passos:" -ForegroundColor Cyan
Write-Host "   1. Execute: dotnet ef database update" -ForegroundColor White
Write-Host "   2. Execute: dotnet run" -ForegroundColor White
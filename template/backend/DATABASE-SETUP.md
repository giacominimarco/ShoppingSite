# 🗄️ Configuração do Banco de Dados PostgreSQL

## 📋 Pré-requisitos

1. **Docker Desktop** instalado e rodando
2. **.NET 8.0 SDK** instalado
3. **Entity Framework CLI** instalado

## 🚀 Configuração Rápida (Recomendado)

### Passo 1: Executar o script de configuração
```powershell
# No diretório template/backend
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
.\setup-database.ps1
```

### Passo 2: Executar as migrações
```powershell
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update
```

### Passo 3: Testar a aplicação
```powershell
dotnet run
```

## 🔧 Configuração Manual

### Opção 1: Docker (Recomendado)

1. **Iniciar o PostgreSQL via Docker:**
```powershell
docker-compose up -d ambev.developerevaluation.database
```

2. **Verificar se está rodando:**
```powershell
docker ps
```

3. **Testar conexão:**
```powershell
Test-NetConnection -ComputerName localhost -Port 5432
```

### Opção 2: PostgreSQL Local

1. **Instalar PostgreSQL:**
   - Baixe em: https://www.postgresql.org/download/windows/
   - Senha: `ev@luAt10n`
   - Porta: `5432`

2. **Criar banco de dados:**
```sql
CREATE DATABASE developer_evaluation;
CREATE USER developer WITH PASSWORD 'ev@luAt10n';
GRANT ALL PRIVILEGES ON DATABASE developer_evaluation TO developer;
```

## 🔍 Verificação da Conexão

### Teste 1: Verificar se o PostgreSQL está rodando
```powershell
docker ps --filter "name=ambev_developer_evaluation_database"
```

### Teste 2: Testar conectividade
```powershell
Test-NetConnection -ComputerName localhost -Port 5432
```

### Teste 3: Executar migrações
```powershell
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update
```

## 📊 Configurações do Banco

| Configuração | Valor |
|--------------|-------|
| **Host** | localhost |
| **Porta** | 5432 |
| **Database** | developer_evaluation |
| **Usuário** | developer |
| **Senha** | ev@luAt10n |

## 🛠️ Solução de Problemas

### Erro: "Docker Desktop não está rodando"
1. Abra o Docker Desktop
2. Aguarde até aparecer "Docker Desktop is running"
3. Execute novamente o script

### Erro: "Failed to connect to 127.0.0.1:5432"
1. Verifique se o PostgreSQL está rodando: `docker ps`
2. Aguarde alguns segundos para o container inicializar
3. Teste a conexão: `Test-NetConnection -ComputerName localhost -Port 5432`

### Erro: "Database does not exist"
1. Execute as migrações: `dotnet ef database update`
2. Verifique se o banco foi criado: `docker exec -it ambev_developer_evaluation_database psql -U developer -d developer_evaluation`

## 📝 Comandos Úteis

```powershell
# Parar todos os containers
docker-compose down

# Ver logs do PostgreSQL
docker logs ambev_developer_evaluation_database

# Acessar o PostgreSQL via CLI
docker exec -it ambev_developer_evaluation_database psql -U developer -d developer_evaluation

# Remover e recriar o banco
docker-compose down -v
docker-compose up -d ambev.developerevaluation.database
```

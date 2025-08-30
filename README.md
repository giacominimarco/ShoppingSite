# Developer Evaluation Project

> **🚀 [Clique aqui para ir direto para Configuração e Execução](#-configuração-e-execução)**

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](/.doc/general-api.md)
- [Sales API](/.doc/sales-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)

## Frontend React
O projeto inclui um frontend React completo para consumir as APIs desenvolvidas:
- [Frontend React](template/frontend/README.md) - Documentação completa do frontend
- Interface moderna e responsiva
- Formulários dinâmicos para criação de vendas
- Listagem com filtros e paginação
- Visualização detalhada de vendas
- Implementação das regras de negócio no frontend

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)

## 🚀 Configuração e Execução

### 📋 Pré-requisitos

- **.NET 8.0 SDK**
- **Node.js 16+** (para frontend)
- **PostgreSQL 12+** ou **Docker**
- **Git**

### 🗄️ Configuração do Banco de Dados

#### Opção 1: Docker (Recomendado)

```bash
# Na raiz do projeto
cd template/backend

# Executar o script de configuração (Windows PowerShell)
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
.\setup-database.ps1

# Ou usar docker-compose
docker-compose up -d ambev.developerevaluation.database
```

#### Opção 2: PostgreSQL Local

1. Instale PostgreSQL em: https://www.postgresql.org/download/
2. Crie o banco `developer_evaluation`
3. Configure a string de conexão em `appsettings.json`

### ⚙️ Configuração do Backend

```bash
# Navegar para o backend
cd template/backend

# Restaurar dependências
dotnet restore

# Build do projeto
dotnet build

# Aplicar migrações
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update

# Executar a aplicação
dotnet run
```

### ⚛️ Configuração do Frontend

```bash
# Navegar para o frontend
cd template/frontend

# Instalar dependências
npm install

# Configurar variáveis de ambiente (opcional)
# O frontend detecta automaticamente a porta correta:
# - HTTP: http://localhost:5119/api
# - HTTPS: https://localhost:7181/api

# Se quiser forçar uma URL específica, crie um arquivo .env:
# REACT_APP_API_URL=http://localhost:5119/api

# Executar em desenvolvimento
npm start
```

**URL do Frontend**: http://localhost:3000

### 🔗 **Detecção Automática de URLs**

O frontend detecta automaticamente a porta correta do backend:
- **HTTP**: `http://localhost:5119/api`
- **HTTPS**: `https://localhost:7181/api`

## 🏗️ Estrutura do Projeto

```
template/
├── backend/                          # Backend .NET
│   ├── src/
│   │   ├── Ambev.DeveloperEvaluation.Domain/     # Entidades e lógica de negócio
│   │   ├── Ambev.DeveloperEvaluation.Application/ # Serviços e comandos
│   │   ├── Ambev.DeveloperEvaluation.ORM/        # Camada de acesso a dados
│   │   ├── Ambev.DeveloperEvaluation.WebApi/     # Controllers e endpoints
│   │   └── Ambev.DeveloperEvaluation.IoC/        # Injeção de dependências
│   ├── tests/                        # Testes unitários e de integração
│   └── docker-compose.yml           # Configuração Docker
└── frontend/                         # Frontend React
    ├── src/
    │   ├── components/              # Componentes reutilizáveis
    │   ├── pages/                   # Páginas da aplicação
    │   ├── services/                # Serviços de API
    │   └── types/                   # Tipos TypeScript
    └── package.json                 # Dependências
```

## 🎨 Funcionalidades do Frontend

### 1. Página Inicial (`/`)
- Visão geral do sistema
- Cards de navegação
- Explicação das regras de negócio

### 2. Listagem de Vendas (`/sales`)
- Tabela com todas as vendas
- Filtros por cliente, filial e status
- Paginação
- Ações: visualizar e cancelar

### 3. Detalhes da Venda (`/sales/:id`)
- Informações completas da venda
- Lista de itens com status
- Resumo financeiro
- Cancelamento de itens individuais

### 4. Criação de Venda (`/sales/new`)
- Formulário dinâmico para itens
- Validação em tempo real
- Cálculo automático de totais
- Aplicação de regras de desconto

## 🔧 Desenvolvimento

### Adicionando Novas Funcionalidades

#### Backend
1. Crie entidades no projeto Domain
2. Adicione comandos/handlers no projeto Application
3. Implemente repositórios no projeto ORM
4. Crie controllers no projeto WebApi
5. Registre dependências no projeto IoC

#### Frontend
1. Crie os tipos necessários em `src/types/`
2. Implemente os métodos da API em `src/services/api.ts`
3. Crie os componentes em `src/components/` ou `src/pages/`
4. Adicione as rotas em `src/App.tsx`

### Executando Testes

```bash
# Backend
cd template/backend
dotnet test

# Frontend
cd template/frontend
npm test
```

## 🐳 Deploy com Docker

### Backend
```bash
cd template/backend
docker build -t sales-api .
docker run -p 8080:80 -e ConnectionStrings__DefaultConnection="sua_string_conexao" sales-api
```

### Frontend
```bash
cd template/frontend
npm run build
# Os arquivos estarão na pasta build/
```

## 🛠️ Solução de Problemas

### Problemas Comuns

1. **Erro de Conexão com Banco**
   - Verifique se o PostgreSQL está rodando
   - Confirme a string de conexão em `appsettings.json`
   - Execute as migrações: `dotnet ef database update`

2. **Porta Já em Uso**
   - Altere a porta em `launchSettings.json`
   - Ou encerre o processo usando a porta

3. **Erro de CORS**
   - Verifique se o backend está configurado para aceitar requisições do frontend
   - Confirme a URL da API no arquivo `.env`

4. **Erro de SSL**
   - Execute: `dotnet dev-certs https --trust`
   - Ou use HTTP em desenvolvimento


## 🎯 Características Implementadas

### ✅ Funcionalidades Core
- CRUD completo de vendas
- Regras de negócio para descontos
- Validações de quantidade
- Sistema de eventos (logs)
- Interface React responsiva
- Filtros e paginação
- Cancelamento de vendas e itens

### ✅ Arquitetura
- DDD com CQRS
- Injeção de dependências
- Repository pattern
- Validação com FluentValidation
- Mapeamento com AutoMapper
- Testes unitários

### ✅ Frontend
- Formulários dinâmicos
- Validação em tempo real
- Cálculos automáticos
- Design responsivo
- Integração completa com API

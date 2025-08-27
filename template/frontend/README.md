# Frontend React - Sistema de Vendas

Este é o frontend React para o Sistema de Gerenciamento de Vendas, desenvolvido para consumir as APIs do backend .NET.

## Tecnologias Utilizadas

- **React 18** - Biblioteca JavaScript para construção de interfaces
- **TypeScript** - Superset do JavaScript com tipagem estática
- **React Router DOM** - Roteamento para aplicações React
- **React Hook Form** - Gerenciamento de formulários
- **Yup** - Validação de esquemas
- **Axios** - Cliente HTTP para requisições à API
- **Lucide React** - Biblioteca de ícones
- **Tailwind CSS** - Framework CSS utilitário

## Funcionalidades

### 1. Página Inicial
- Visão geral do sistema
- Cards de navegação para principais funcionalidades
- Explicação das regras de negócio

### 2. Listagem de Vendas
- Tabela com todas as vendas
- Filtros por cliente, filial e status
- Paginação
- Ações: visualizar detalhes e cancelar vendas

### 3. Detalhes da Venda
- Informações completas da venda
- Lista de itens com valores
- Ação de cancelamento
- Resumo financeiro

### 4. Criação de Nova Venda
- Formulário dinâmico para adicionar itens
- Validação em tempo real
- Cálculo automático de totais
- Aplicação de regras de desconto

## Regras de Negócio Implementadas

### Descontos por Quantidade
- **1-3 itens**: Sem desconto
- **4-9 itens**: 10% de desconto automático
- **10-19 itens**: 20% de desconto automático
- **20+ itens**: Não permitido

### Validações
- Quantidade máxima de 19 itens por produto
- Descontos manuais não permitidos para quantidades 1-3
- Campos obrigatórios validados
- Formatação de moeda brasileira

## Estrutura do Projeto

```
src/
├── components/          # Componentes reutilizáveis
│   └── Layout.tsx      # Layout principal da aplicação
├── pages/              # Páginas da aplicação
│   ├── Home.tsx        # Página inicial
│   ├── SalesList.tsx   # Listagem de vendas
│   ├── SaleDetail.tsx  # Detalhes da venda
│   └── CreateSale.tsx  # Criação de nova venda
├── services/           # Serviços de API
│   └── api.ts         # Cliente HTTP e métodos da API
├── types/              # Definições de tipos TypeScript
│   └── api.ts         # Interfaces da API
├── App.tsx            # Componente principal com rotas
└── index.css          # Estilos globais com Tailwind
```

## Configuração

### Variáveis de Ambiente
Crie um arquivo `.env` na raiz do projeto:

```env
REACT_APP_API_URL=https://localhost:7001/api
```

### Instalação e Execução

1. **Instalar dependências:**
   ```bash
   npm install
   ```

2. **Executar em modo de desenvolvimento:**
   ```bash
   npm start
   ```

3. **Build para produção:**
   ```bash
   npm run build
   ```

4. **Executar testes:**
   ```bash
   npm test
   ```

## Rotas da Aplicação

- `/` - Página inicial
- `/sales` - Listagem de vendas
- `/sales/new` - Criação de nova venda
- `/sales/:id` - Detalhes de uma venda específica

## Integração com Backend

O frontend se comunica com o backend através das seguintes APIs:

- `POST /api/sales` - Criar nova venda
- `GET /api/sales` - Listar vendas com filtros
- `GET /api/sales/{id}` - Obter detalhes de uma venda
- `POST /api/sales/{id}/cancel` - Cancelar uma venda

## Características Técnicas

### Formulários
- Uso do React Hook Form para gerenciamento eficiente
- Validação com Yup
- Campos dinâmicos para itens de venda
- Cálculos automáticos de totais

### Interface
- Design responsivo com Tailwind CSS
- Componentes reutilizáveis
- Ícones da biblioteca Lucide React
- Feedback visual para ações do usuário

### Estado e Dados
- Gerenciamento de estado local com React Hooks
- Comunicação assíncrona com APIs
- Tratamento de erros
- Loading states

## Desenvolvimento

### Adicionando Novas Funcionalidades
1. Crie os tipos necessários em `src/types/`
2. Implemente os métodos da API em `src/services/api.ts`
3. Crie os componentes em `src/components/` ou `src/pages/`
4. Adicione as rotas em `src/App.tsx`

### Estilização
- Use classes do Tailwind CSS
- Mantenha consistência com o design system
- Priorize componentes reutilizáveis

### Validação
- Use Yup para validação de esquemas
- Implemente validação tanto no frontend quanto no backend
- Forneça feedback claro para o usuário

# Configuração e Execução do Frontend React

## Pré-requisitos

- Node.js (versão 16 ou superior)
- npm ou yarn
- Backend .NET executando na porta 7001

## Configuração Inicial

### 1. Instalar Dependências

```bash
cd template/frontend
npm install
```

### 2. Configurar Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto frontend:

```env
REACT_APP_API_URL=https://localhost:7001/api
```

**Nota**: Se o backend estiver rodando em uma porta diferente, ajuste a URL conforme necessário.

### 3. Executar o Frontend

```bash
npm start
```

O frontend será aberto automaticamente em `http://localhost:3000`

## Estrutura de Arquivos

```
frontend/
├── src/
│   ├── components/          # Componentes reutilizáveis
│   │   └── Layout.tsx      # Layout principal
│   ├── pages/              # Páginas da aplicação
│   │   ├── Home.tsx        # Página inicial
│   │   ├── SalesList.tsx   # Listagem de vendas
│   │   ├── SaleDetail.tsx  # Detalhes da venda
│   │   └── CreateSale.tsx  # Criação de nova venda
│   ├── services/           # Serviços de API
│   │   └── api.ts         # Cliente HTTP
│   ├── types/              # Tipos TypeScript
│   │   └── api.ts         # Interfaces da API
│   ├── App.tsx            # Componente principal
│   └── index.css          # Estilos globais
├── public/                 # Arquivos públicos
├── package.json           # Dependências
├── tailwind.config.js     # Configuração Tailwind
└── tsconfig.json          # Configuração TypeScript
```

## Funcionalidades Implementadas

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
- Lista de itens
- Resumo financeiro
- Ação de cancelamento

### 4. Criação de Venda (`/sales/new`)
- Formulário dinâmico
- Validação em tempo real
- Cálculo automático de totais
- Regras de desconto

## Regras de Negócio

### Descontos por Quantidade
- **1-3 itens**: Sem desconto
- **4-9 itens**: 10% de desconto automático
- **10-19 itens**: 20% de desconto automático
- **20+ itens**: Não permitido

### Validações
- Quantidade máxima: 19 itens por produto
- Descontos manuais não permitidos para quantidades 1-3
- Campos obrigatórios validados
- Formatação de moeda brasileira

## Comandos Disponíveis

```bash
# Executar em desenvolvimento
npm start

# Build para produção
npm run build

# Executar testes
npm test

# Ejetar configurações (não recomendado)
npm run eject
```

## Troubleshooting

### Problemas Comuns

1. **Erro de CORS**
   - Certifique-se de que o backend está configurado para aceitar requisições do frontend
   - Verifique se a URL da API está correta no arquivo `.env`

2. **Erro de Conexão**
   - Verifique se o backend está rodando
   - Confirme a porta e URL da API

3. **Erro de Dependências**
   - Delete a pasta `node_modules` e `package-lock.json`
   - Execute `npm install` novamente

4. **Erro de Build**
   - Verifique se todas as dependências estão instaladas
   - Confirme se o TypeScript está configurado corretamente

### Logs de Desenvolvimento

O React Developer Tools pode ser útil para debug:
- Instale a extensão no Chrome/Firefox
- Use o console do navegador para ver logs
- Verifique a aba Network para requisições HTTP

## Integração com Backend

O frontend se comunica com as seguintes APIs:

- `POST /api/sales` - Criar venda
- `GET /api/sales` - Listar vendas
- `GET /api/sales/{id}` - Obter venda
- `POST /api/sales/{id}/cancel` - Cancelar venda

## Deploy

Para fazer deploy em produção:

1. Configure as variáveis de ambiente para a URL de produção
2. Execute `npm run build`
3. Os arquivos estarão na pasta `build/`
4. Faça upload dos arquivos para seu servidor web

## Contribuição

Para adicionar novas funcionalidades:

1. Crie os tipos necessários em `src/types/`
2. Implemente os métodos da API em `src/services/api.ts`
3. Crie os componentes em `src/components/` ou `src/pages/`
4. Adicione as rotas em `src/App.tsx`

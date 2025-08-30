import React from 'react';
import { Link } from 'react-router-dom';
import { List, Plus, TrendingUp } from 'lucide-react';

const Home: React.FC = () => {
  return (
    <div className="px-4 sm:px-6 lg:px-8">
      {/* Hero Section */}
      <div className="text-center mb-12">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          Sistema de Gerenciamento de Vendas
        </h1>
        <p className="text-xl text-gray-600 max-w-3xl mx-auto">
          Gerencie suas vendas de forma eficiente com nosso sistema completo. 
          Crie, visualize e gerencie vendas com facilidade.
        </p>
      </div>

      {/* Features Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-12">
        <div className="bg-white p-6 rounded-lg shadow-md border">
          <div className="flex items-center mb-4">
            <Plus className="h-8 w-8 text-blue-600 mr-3" />
            <h3 className="text-lg font-semibold text-gray-900">Nova Venda</h3>
          </div>
          <p className="text-gray-600 mb-4">
            Crie uma nova venda com múltiplos itens, aplicando descontos automáticos baseados na quantidade.
          </p>
          <Link
            to="/sales/new"
            className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700"
          >
            Criar Venda
          </Link>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-md border">
          <div className="flex items-center mb-4">
            <List className="h-8 w-8 text-green-600 mr-3" />
            <h3 className="text-lg font-semibold text-gray-900">Listar Vendas</h3>
          </div>
          <p className="text-gray-600 mb-4">
            Visualize todas as vendas com filtros avançados, paginação e ordenação.
          </p>
          <Link
            to="/sales"
            className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-green-600 hover:bg-green-700"
          >
            Ver Vendas
          </Link>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-md border">
          <div className="flex items-center mb-4">
            <TrendingUp className="h-8 w-8 text-purple-600 mr-3" />
            <h3 className="text-lg font-semibold text-gray-900">Relatórios</h3>
          </div>
          <p className="text-gray-600 mb-4">
            Acompanhe o desempenho das vendas com relatórios detalhados e análises.
          </p>
          <button className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-purple-600 hover:bg-purple-700">
            Ver Relatórios
          </button>
        </div>
      </div>

      {/* Business Rules Section */}
      <div className="bg-white p-6 rounded-lg shadow-md border">
        <h2 className="text-2xl font-bold text-gray-900 mb-4">Regras de Negócio</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Descontos por Quantidade</h3>
            <ul className="text-gray-600 space-y-1">
              <li>• Quantidade 1-3: Sem desconto</li>
              <li>• Quantidade 4-9: 10% de desconto</li>
              <li>• Quantidade 10-19: 20% de desconto</li>
              <li>• Quantidade 20+: Não permitido</li>
            </ul>
          </div>
          <div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Funcionalidades</h3>
            <ul className="text-gray-600 space-y-1">
              <li>• Criação de vendas com múltiplos itens</li>
              <li>• Cancelamento de vendas completas</li>
              <li>• Cancelamento de itens individuais</li>
              <li>• Filtros e paginação avançados</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;

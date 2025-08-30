import React, { useState, useEffect, useCallback } from 'react';
import { useParams, Link } from 'react-router-dom';
import { ArrowLeft, X } from 'lucide-react';
import { salesApi } from '../services/api';
import { Sale } from '../types/api';

const SaleDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [sale, setSale] = useState<Sale | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadSale = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      console.log('Loading sale with ID:', id);
      const response = await salesApi.getSale(id!);
      console.log('API response:', response);
      setSale(response);
    } catch (err: any) {
      console.error('Error loading sale:', err);
      setError(err.message || 'Erro ao carregar venda');
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    if (id) {
      loadSale();
    }
  }, [id, loadSale]);

  const handleCancelSale = async () => {
    if (!sale || !window.confirm('Tem certeza que deseja cancelar esta venda?')) {
      return;
    }

    try {
      await salesApi.cancelSale(sale.id);
      await loadSale(); // Reload the sale data
    } catch (err: any) {
      alert(err.message || 'Erro ao cancelar venda');
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-lg">Carregando venda...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="px-4 sm:px-6 lg:px-8">
        <div className="bg-red-50 border border-red-200 rounded-md p-4">
          <div className="text-red-800">{error}</div>
          <Link
            to="/sales"
            className="mt-4 inline-flex items-center text-red-600 hover:text-red-800"
          >
            <ArrowLeft className="h-4 w-4 mr-1" />
            Voltar para vendas
          </Link>
        </div>
      </div>
    );
  }

  if (!sale) {
    return (
      <div className="px-4 sm:px-6 lg:px-8">
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg">Venda não encontrada.</p>
          <Link
            to="/sales"
            className="mt-4 inline-flex items-center text-blue-600 hover:text-blue-800"
          >
            <ArrowLeft className="h-4 w-4 mr-1" />
            Voltar para vendas
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      {/* Header */}
      <div className="mb-8">
        <Link
          to="/sales"
          className="inline-flex items-center text-gray-600 hover:text-gray-900 mb-4"
        >
          <ArrowLeft className="h-4 w-4 mr-1" />
          Voltar para vendas
        </Link>
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">
              Venda #{sale.saleNumber}
            </h1>
            <p className="text-gray-600 mt-1">
              Criada em {formatDate(sale.createdAt)}
            </p>
          </div>
          <div className="flex items-center space-x-3">
            <span
              className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${
                sale.status === 'Active'
                  ? 'bg-green-100 text-green-800'
                  : 'bg-red-100 text-red-800'
              }`}
            >
              {sale.status === 'Active' ? 'Ativo' : 'Cancelado'}
            </span>
            {sale.status === 'Active' && (
              <button
                onClick={handleCancelSale}
                className="inline-flex items-center px-4 py-2 border border-red-300 shadow-sm text-sm font-medium rounded-md text-red-700 bg-white hover:bg-red-50"
              >
                <X className="h-4 w-4 mr-1" />
                Cancelar Venda
              </button>
            )}
          </div>
        </div>
      </div>

      {/* Sale Information */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
        <div className="bg-white p-6 rounded-lg shadow border">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Informações da Venda</h2>
          <dl className="space-y-3">
            <div>
              <dt className="text-sm font-medium text-gray-500">Número da Venda</dt>
              <dd className="text-sm text-gray-900">{sale.saleNumber}</dd>
            </div>
            <div>
              <dt className="text-sm font-medium text-gray-500">Data da Venda</dt>
              <dd className="text-sm text-gray-900">{formatDate(sale.saleDate)}</dd>
            </div>
            <div>
              <dt className="text-sm font-medium text-gray-500">Cliente</dt>
              <dd className="text-sm text-gray-900">{sale.customer}</dd>
            </div>
            <div>
              <dt className="text-sm font-medium text-gray-500">Filial</dt>
              <dd className="text-sm text-gray-900">{sale.branch}</dd>
            </div>
            <div>
              <dt className="text-sm font-medium text-gray-500">Status</dt>
              <dd className="text-sm text-gray-900">
                <span
                  className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                    sale.status === 'Active'
                      ? 'bg-green-100 text-green-800'
                      : 'bg-red-100 text-red-800'
                  }`}
                >
                  {sale.status === 'Active' ? 'Ativo' : 'Cancelado'}
                </span>
              </dd>
            </div>
          </dl>
        </div>

        <div className="bg-white p-6 rounded-lg shadow border">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Resumo Financeiro</h2>
          <dl className="space-y-3">
            <div>
              <dt className="text-sm font-medium text-gray-500">Total de Itens</dt>
              <dd className="text-sm text-gray-900">{sale.items.length}</dd>
            </div>
            <div>
              <dt className="text-sm font-medium text-gray-500">Valor Total</dt>
              <dd className="text-2xl font-bold text-gray-900">
                {formatCurrency(sale.totalAmount)}
              </dd>
            </div>
            {sale.updatedAt && (
              <div>
                <dt className="text-sm font-medium text-gray-500">Última Atualização</dt>
                <dd className="text-sm text-gray-900">{formatDate(sale.updatedAt)}</dd>
              </div>
            )}
          </dl>
        </div>
      </div>

      {/* Items List */}
      <div className="bg-white rounded-lg shadow border">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-lg font-semibold text-gray-900">Itens da Venda</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Produto
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Quantidade
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Preço Unitário
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Desconto
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Total
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {sale.items.map((item) => (
                <tr key={item.id}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {item.product}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {item.quantity}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {formatCurrency(item.unitPrice)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {item.discount > 0 ? formatCurrency(item.discount) : '-'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {formatCurrency(item.totalAmount)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default SaleDetail;

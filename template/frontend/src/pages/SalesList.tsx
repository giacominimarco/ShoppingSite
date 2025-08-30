import React, { useState, useEffect, useCallback, useRef } from 'react';
import { Link } from 'react-router-dom';
import { Eye, X, Filter, ChevronLeft, ChevronRight } from 'lucide-react';
import { salesApi } from '../services/api';
import { Sale, GetSalesRequest } from '../types/api';

const SalesList: React.FC = () => {
  const [sales, setSales] = useState<Sale[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  
  // Estado local para os filtros (atualização imediata na UI)
  const [localFilters, setLocalFilters] = useState<GetSalesRequest>({
    page: 1,
    size: 10,
  });
  
  // Estado para os filtros de busca (usado para fazer a requisição)
  const [searchFilters, setSearchFilters] = useState<GetSalesRequest>({
    page: 1,
    size: 10,
  });

  // Ref para o timeout do debounce
  const debounceTimeoutRef = useRef<NodeJS.Timeout | undefined>(undefined);

  const loadSales = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      console.log('🔍 Carregando vendas com filtros:', searchFilters);
      console.log('🔗 URL da API:', `${process.env.REACT_APP_API_URL || 'http://localhost:5119'}/api/sales`);
      
      const response = await salesApi.getSales(searchFilters);
      console.log('📡 Resposta completa da API:', response);
      console.log('📊 Dados da resposta:', response);
      console.log('🔍 Estrutura completa:', JSON.stringify(response, null, 2));
      
      // A API retorna {data: {...}}, mas o tipo GetSalesResponse não tem essa propriedade
      // Vamos verificar se a resposta tem a estrutura esperada
      if (response && typeof response === 'object') {
        // Se a resposta tem a propriedade 'data', use-a; caso contrário, use a resposta diretamente
        const salesData = (response as any).data || response;
        setSales(salesData.sales || []);
        setTotalPages(salesData.totalPages || 1);
        setTotalCount(salesData.totalCount || 0);
        
        console.log('✅ Vendas carregadas:', salesData.sales?.length || 0);
      } else {
        console.error('❌ Resposta da API inválida:', response);
        setError('Resposta da API inválida');
        setSales([]);
        setTotalPages(1);
        setTotalCount(0);
      }
    } catch (err: any) {
      console.error('❌ Erro ao carregar vendas:', err);
      console.error('❌ Detalhes do erro:', err.response?.data || err.message);
      setError(err.message || 'Erro ao carregar vendas');
      setSales([]);
      setTotalPages(1);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [searchFilters]);

  useEffect(() => {
    loadSales();
  }, [loadSales]);

  // Função para aplicar filtros imediatamente (para campos que não precisam de debounce)
  const handleFilterChange = (key: keyof GetSalesRequest, value: string | number) => {
    const newFilters = {
      ...localFilters,
      [key]: value,
      page: 1, // Reset to first page when filtering
    };
    
    setLocalFilters(newFilters);
    setSearchFilters(newFilters); // Aplicar imediatamente
  };

  // Função para campos de valor com debounce
  const handleAmountFilterChange = (key: 'minTotalAmount' | 'maxTotalAmount', value: string) => {
    const numericValue = value ? parseFloat(value) : undefined;
    
    // Atualizar o estado local imediatamente para feedback visual
    setLocalFilters(prev => ({
      ...prev,
      [key]: numericValue,
    }));

    // Cancelar timeout anterior se existir
    if (debounceTimeoutRef.current) {
      clearTimeout(debounceTimeoutRef.current);
    }

    // Criar novo timeout para aplicar o filtro de busca
    debounceTimeoutRef.current = setTimeout(() => {
      setSearchFilters(prev => ({
        ...prev,
        [key]: numericValue,
        page: 1, // Reset to first page when filtering
      }));
    }, 500);
  };

  const handlePageChange = (page: number) => {
    const newFilters = { ...searchFilters, page };
    setLocalFilters(newFilters);
    setSearchFilters(newFilters);
  };

  const handleCancelSale = async (saleId: string) => {
    if (window.confirm('Tem certeza que deseja cancelar esta venda?')) {
      try {
        await salesApi.cancelSale(saleId);
        loadSales(); // Reload the list
      } catch (err: any) {
        alert(err.message || 'Erro ao cancelar venda');
      }
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  // Cleanup do timeout quando o componente for desmontado
  useEffect(() => {
    return () => {
      if (debounceTimeoutRef.current) {
        clearTimeout(debounceTimeoutRef.current);
      }
    };
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-lg">Carregando vendas...</div>
      </div>
    );
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      <div className="sm:flex sm:items-center">
        <div className="sm:flex-auto">
          <h1 className="text-2xl font-semibold text-gray-900">Vendas</h1>
          <p className="mt-2 text-sm text-gray-700">
            Lista de todas as vendas realizadas no sistema.
          </p>
        </div>
        <div className="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
          <Link
            to="/sales/new"
            className="inline-flex items-center justify-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-blue-700"
          >
            Nova Venda
          </Link>
        </div>
      </div>

      {/* Filters */}
      <div className="mt-8 bg-white p-4 rounded-lg shadow border">
        <div className="flex items-center mb-4">
          <Filter className="h-5 w-5 text-gray-400 mr-2" />
          <h3 className="text-lg font-medium text-gray-900">Filtros</h3>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-6 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Cliente
            </label>
            <input
              type="text"
              value={localFilters.customer || ''}
              onChange={(e) => handleFilterChange('customer', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Nome do cliente"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Filial
            </label>
            <input
              type="text"
              value={localFilters.branch || ''}
              onChange={(e) => handleFilterChange('branch', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Nome da filial"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Status
            </label>
            <select
              value={localFilters.status || ''}
              onChange={(e) => handleFilterChange('status', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">Todos</option>
              <option value="Active">Ativo</option>
              <option value="Cancelled">Cancelado</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Data Inicial
            </label>
            <input
              type="date"
              value={localFilters.minDate || ''}
              onChange={(e) => handleFilterChange('minDate', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Data Final
            </label>
            <input
              type="date"
              value={localFilters.maxDate || ''}
              onChange={(e) => handleFilterChange('maxDate', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Itens por página
            </label>
            <select
              value={localFilters.size || 10}
              onChange={(e) => handleFilterChange('size', parseInt(e.target.value))}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value={5}>5</option>
              <option value={10}>10</option>
              <option value={20}>20</option>
              <option value={50}>50</option>
            </select>
          </div>
        </div>
        
        {/* Additional filters row */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mt-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Valor Mínimo
            </label>
            <input
              type="number"
              step="0.01"
              value={localFilters.minTotalAmount || ''}
              onChange={(e) => handleAmountFilterChange('minTotalAmount', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="0.00"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Valor Máximo
            </label>
            <input
              type="number"
              step="0.01"
              value={localFilters.maxTotalAmount || ''}
              onChange={(e) => handleAmountFilterChange('maxTotalAmount', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="999999.99"
            />
          </div>
          <div className="flex items-end">
            <button
              onClick={() => {
                const resetFilters = { page: 1, size: 10 };
                setLocalFilters(resetFilters);
                setSearchFilters(resetFilters);
              }}
              className="w-full px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-500"
            >
              Limpar Filtros
            </button>
          </div>
        </div>
      </div>

      {/* Error Message */}
      {error && (
        <div className="mt-4 bg-red-50 border border-red-200 rounded-md p-4">
          <div className="text-red-800">{error}</div>
        </div>
      )}

      {/* Sales Table */}
      <div className="mt-8 bg-white shadow overflow-hidden sm:rounded-md">
        <ul className="divide-y divide-gray-200">
          {sales && sales.map((sale) => (
            <li key={sale.id} className="px-6 py-4">
              <div className="flex items-center justify-between">
                <div className="flex-1 min-w-0">
                  <div className="flex items-center justify-between">
                    <div>
                      <p className="text-sm font-medium text-gray-900 truncate">
                        Venda #{sale.saleNumber}
                      </p>
                      <p className="text-sm text-gray-500">
                        Cliente: {sale.customer} | Filial: {sale.branch}
                      </p>
                      <p className="text-sm text-gray-500">
                        Data: {formatDate(sale.saleDate)}
                      </p>
                    </div>
                    <div className="text-right">
                      <p className="text-lg font-semibold text-gray-900">
                        {formatCurrency(sale.totalAmount)}
                      </p>
                      <span
                        className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                          sale.status === 'Active'
                            ? 'bg-green-100 text-green-800'
                            : 'bg-red-100 text-red-800'
                        }`}
                      >
                        {sale.status === 'Active' ? 'Ativo' : 'Cancelado'}
                      </span>
                    </div>
                  </div>
                </div>
                <div className="ml-4 flex items-center space-x-2">
                  <Link
                    to={`/sales/${sale.id}`}
                    className="inline-flex items-center px-3 py-1 border border-gray-300 shadow-sm text-sm leading-4 font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
                  >
                    <Eye className="h-4 w-4 mr-1" />
                    Ver
                  </Link>
                  {sale.status === 'Active' && (
                    <button
                      onClick={() => handleCancelSale(sale.id)}
                      className="inline-flex items-center px-3 py-1 border border-red-300 shadow-sm text-sm leading-4 font-medium rounded-md text-red-700 bg-white hover:bg-red-50"
                    >
                      <X className="h-4 w-4 mr-1" />
                      Cancelar
                    </button>
                  )}
                </div>
              </div>
            </li>
          ))}
        </ul>
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="mt-8 flex items-center justify-between">
          <div className="text-sm text-gray-700">
            Mostrando {(((searchFilters.page || 1) - 1) * (searchFilters.size || 10)) + 1} a{' '}
            {Math.min((searchFilters.page || 1) * (searchFilters.size || 10), totalCount)} de {totalCount} resultados
          </div>
          <div className="flex items-center space-x-2">
            <button
              onClick={() => handlePageChange((searchFilters.page || 1) - 1)}
              disabled={(searchFilters.page || 1) === 1}
              className="inline-flex items-center px-3 py-2 border border-gray-300 shadow-sm text-sm leading-4 font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <ChevronLeft className="h-4 w-4" />
              Anterior
            </button>
            <span className="text-sm text-gray-700">
              Página {searchFilters.page || 1} de {totalPages}
            </span>
            <button
              onClick={() => handlePageChange((searchFilters.page || 1) + 1)}
              disabled={(searchFilters.page || 1) === totalPages}
              className="inline-flex items-center px-3 py-2 border border-gray-300 shadow-sm text-sm leading-4 font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Próxima
              <ChevronRight className="h-4 w-4" />
            </button>
          </div>
        </div>
      )}

      {sales && sales.length === 0 && !loading && (
        <div className="mt-8 text-center py-12">
          <p className="text-gray-500 text-lg">Nenhuma venda encontrada.</p>
        </div>
      )}
    </div>
  );
};

export default SalesList;

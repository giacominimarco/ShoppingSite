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
  const [cancellingItems, setCancellingItems] = useState<Set<string>>(new Set());

  const loadSale = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      console.log('Loading sale with ID:', id);
      const response = await salesApi.getSale(id!);
      console.log('API response:', response);
      
      // A API retorna {data: {...}}, mas o tipo Sale não tem essa propriedade
      // Vamos verificar se a resposta tem a estrutura esperada
      if (response && typeof response === 'object') {
        // Se a resposta tem a propriedade 'data', use-a; caso contrário, use a resposta diretamente
        const saleData = (response as any).data || response;
        console.log('Sale data:', saleData);
        console.log('Sale status:', saleData.status);
        console.log('Sale items:', saleData.items);
        if (saleData.items) {
          saleData.items.forEach((item: any, index: number) => {
            console.log(`Item ${index}:`, item);
            console.log(`Item ${index} status:`, item.status);
          });
        }
        setSale(saleData);
      } else {
        console.error('❌ Resposta da API inválida:', response);
        setError('Resposta da API inválida');
      }
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

  // Função para cancelar um item específico
  const handleCancelItem = async (itemId: string, productName: string) => {
    if (!sale || !window.confirm(`Tem certeza que deseja cancelar o item "${productName}"?`)) {
      return;
    }

    try {
      // Adicionar item ao conjunto de itens sendo cancelados
      setCancellingItems(prev => new Set(prev).add(itemId));
      
      const response = await salesApi.cancelSaleItem(sale.id, itemId);
      
      // Mostrar mensagem informativa
      if (response.data.wasAutomaticallyCancelled) {
        alert(`Item "${productName}" cancelado com sucesso!\n\nA venda foi automaticamente cancelada por não ter mais itens.`);
      } else {
        alert(`Item "${productName}" cancelado com sucesso!`);
      }
      
      // Recarregar os dados da venda para atualizar o status
      await loadSale();
    } catch (err: any) {
      alert(err.message || 'Erro ao cancelar item');
    } finally {
      // Remover item do conjunto de itens sendo cancelados
      setCancellingItems(prev => {
        const newSet = new Set(prev);
        newSet.delete(itemId);
        return newSet;
      });
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
                             <dd className="text-sm text-gray-900">
                 {sale.items?.filter(item => item.status === 1).length || 0} ativos
                 {sale.items?.filter(item => item.status === 2).length > 0 && (
                   <span className="text-gray-500 ml-2">
                     / {sale.items?.filter(item => item.status === 2).length} cancelados
                   </span>
                 )}
               </dd>
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
          <div className="flex items-center justify-between">
            <h2 className="text-lg font-semibold text-gray-900">Itens da Venda</h2>
                         {sale.items?.some(item => item.status === 2) && (
               <div className="flex items-center text-sm text-gray-600">
                 <div className="w-2 h-2 bg-red-400 rounded-full mr-2"></div>
                 Itens cancelados são exibidos com fundo cinza e riscados
               </div>
             )}
          </div>
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
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Ações
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {sale.items && sale.items.length > 0 ? (
                sale.items.map((item) => {
                                     // Converter o status numérico para string
                   const itemStatus = item.status === 1 ? 'Active' : item.status === 2 ? 'Removed' : 'Active';
                   const isItemCancelled = itemStatus === 'Removed';
                   const isItemBeingCancelled = cancellingItems.has(item.id);
                   console.log('Rendering item:', item.product, 'Status:', item.status, 'ItemStatus:', itemStatus, 'IsCancelled:', isItemCancelled, 'IsBeingCancelled:', isItemBeingCancelled);
                   
                   // Aplicar classes CSS baseadas no estado
                   let rowClass = '';
                   if (item.status === 2) {
                     rowClass = 'bg-gray-50 opacity-60';
                   } else if (isItemBeingCancelled) {
                     rowClass = 'bg-yellow-50 border-l-4 border-l-yellow-400';
                   }
                   console.log(`Item ${item.product}: rowClass="${rowClass}"`);
                  return (
                    <tr 
                      key={item.id} 
                      className={rowClass}
                    >
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                        <div className="flex items-center">
                                                     {(() => {
                             const showIcon = item.status === 2;
                             console.log(`Item ${item.product}: showIcon=${showIcon}`);
                             return showIcon ? <X className="h-4 w-4 text-red-500 mr-2" /> : null;
                           })()}
                           <span className={item.status === 2 ? 'line-through text-gray-500' : ''}>
                             {item.product}
                           </span>
                        </div>
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
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                 {(() => {
                           let statusText, statusClass;
                           
                           if (isItemBeingCancelled) {
                             statusText = 'Processando...';
                             statusClass = 'bg-yellow-100 text-yellow-800';
                           } else if (item.status === 1) {
                             statusText = 'Ativo';
                             statusClass = 'bg-green-100 text-green-800';
                           } else {
                             statusText = 'Cancelado';
                             statusClass = 'bg-red-100 text-red-800';
                           }
                           
                           console.log(`Item ${item.product}: status=${item.status}, itemStatus=${itemStatus}, isBeingCancelled=${isItemBeingCancelled}, statusText=${statusText}`);
                           return (
                             <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${statusClass}`}>
                               {isItemBeingCancelled && (
                                 <div className="animate-spin rounded-full h-2 w-2 border-b-2 border-yellow-600 mr-1"></div>
                               )}
                               {statusText}
                             </span>
                           );
                         })()}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                        {(() => {
                                                     const shouldShowButton = sale.status === 'Active' && item.status === 1;
                           console.log(`Item ${item.product}: sale.status=${sale.status}, item.status=${item.status}, itemStatus=${itemStatus}, shouldShowButton=${shouldShowButton}`);
                                                     const isCancelling = cancellingItems.has(item.id);
                           return shouldShowButton ? (
                             <button
                               onClick={() => handleCancelItem(item.id, item.product)}
                               disabled={isCancelling}
                               className={`inline-flex items-center px-2 py-1 border text-xs font-medium rounded focus:outline-none focus:ring-2 focus:ring-red-500 transition-all duration-200 ${
                                 isCancelling
                                   ? 'border-gray-300 text-gray-400 bg-gray-100 cursor-not-allowed opacity-50'
                                   : 'border-red-300 text-red-700 bg-white hover:bg-red-50 hover:border-red-400'
                               }`}
                             >
                               {isCancelling ? (
                                 <>
                                   <div className="animate-spin rounded-full h-3 w-3 border-b-2 border-gray-400 mr-1"></div>
                                   Cancelando...
                                 </>
                               ) : (
                                 <>
                                   <X className="h-3 w-3 mr-1" />
                                   Cancelar
                                 </>
                               )}
                             </button>
                           ) : null;
                        })()}
                                                 {item.status === 2 && (
                           <span className="text-xs text-gray-500 italic">
                             Item cancelado
                           </span>
                         )}
                      </td>
                    </tr>
                  );
                })
              ) : (
                <tr>
                  <td colSpan={7} className="px-6 py-4 text-center text-sm text-gray-500">
                    Nenhum item encontrado para esta venda
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default SaleDetail;

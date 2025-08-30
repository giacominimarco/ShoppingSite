import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm, useFieldArray } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { Plus, Trash2, ArrowLeft, Save } from 'lucide-react';
import { salesApi } from '../services/api';
import { CreateSaleRequest, CreateSaleItemRequest } from '../types/api';

const schema = yup.object({
  customer: yup.string().required('Cliente é obrigatório').min(2, 'Cliente deve ter pelo menos 2 caracteres'),
  branch: yup.string().required('Filial é obrigatória').min(2, 'Filial deve ter pelo menos 2 caracteres'),
  items: yup.array().of(
    yup.object({
      product: yup.string().required('Produto é obrigatório').min(2, 'Produto deve ter pelo menos 2 caracteres'),
      quantity: yup.number()
        .required('Quantidade é obrigatória')
        .min(1, 'Quantidade deve ser pelo menos 1')
        .max(19, 'Quantidade máxima permitida é 19'),
      unitPrice: yup.number()
        .required('Preço unitário é obrigatório')
        .min(0.01, 'Preço unitário deve ser maior que zero'),
      discount: yup.number()
        .min(0, 'Desconto não pode ser negativo')
        .max(100, 'Desconto máximo é 100%')
        .optional(),
    })
  ).min(1, 'Pelo menos um item é obrigatório'),
});

const CreateSale: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    control,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
    defaultValues: {
      customer: '',
      branch: '',
      items: [{ product: '', quantity: 1, unitPrice: 0, discount: 0 }],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'items',
  });

  const watchedItems = watch('items');

  const calculateItemTotal = (item: CreateSaleItemRequest) => {
    const quantity = item.quantity || 0;
    const unitPrice = item.unitPrice || 0;
    const discount = item.discount || 0;
    
    const subtotal = quantity * unitPrice;
    const discountAmount = (subtotal * discount) / 100;
    return subtotal - discountAmount;
  };

  const calculateTotal = () => {
    return (watchedItems || []).reduce((total, item) => {
      return total + calculateItemTotal(item);
    }, 0);
  };

  const getDiscountInfo = (quantity: number) => {
    if (quantity >= 1 && quantity <= 3) {
      return { text: 'Sem desconto', color: 'text-gray-500' };
    } else if (quantity >= 4 && quantity <= 9) {
      return { text: '10% de desconto', color: 'text-green-600' };
    } else if (quantity >= 10 && quantity <= 19) {
      return { text: '20% de desconto', color: 'text-blue-600' };
    } else {
      return { text: 'Quantidade não permitida', color: 'text-red-600' };
    }
  };

  const onSubmit = async (data: any) => {
    try {
      setLoading(true);
      setError(null);

      // Validate quantity limits
      const invalidItems = data.items.filter((item: any) => item.quantity > 19);
      if (invalidItems.length > 0) {
        setError('Quantidade máxima permitida por item é 19');
        return;
      }

      // Validate discount rules
      const invalidDiscounts = data.items.filter((item: any) => {
        const quantity = item.quantity || 0;
        const discount = item.discount || 0;
        
        if (quantity >= 1 && quantity <= 3 && discount > 0) {
          return true;
        }
        return false;
      });

      if (invalidDiscounts.length > 0) {
        setError('Descontos manuais não são permitidos para quantidades de 1-3 itens');
        return;
      }

      const saleData: CreateSaleRequest = {
        customer: data.customer,
        branch: data.branch,
        items: data.items.map((item: any) => ({
          product: item.product,
          quantity: item.quantity,
          unitPrice: item.unitPrice,
          discount: item.discount || 0,
        })),
      };

      const response = await salesApi.createSale(saleData);
      console.log('Sale created successfully:', response);
      
      alert('Venda criada com sucesso!');
      navigate(`/sales/${response.data.id}`);
    } catch (err: any) {
      console.error('Error creating sale:', err);
      setError(err.message || 'Erro ao criar venda');
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      {/* Header */}
      <div className="mb-8">
        <button
          onClick={() => navigate('/sales')}
          className="inline-flex items-center text-gray-600 hover:text-gray-900 mb-4"
        >
          <ArrowLeft className="h-4 w-4 mr-1" />
          Voltar para vendas
        </button>
        <h1 className="text-3xl font-bold text-gray-900">Nova Venda</h1>
        <p className="text-gray-600 mt-1">
          Crie uma nova venda com múltiplos itens
        </p>
      </div>

      {/* Error Message */}
      {error && (
        <div className="mb-6 bg-red-50 border border-red-200 rounded-md p-4">
          <div className="text-red-800">{error}</div>
        </div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-8">
        {/* Sale Information */}
        <div className="bg-white p-6 rounded-lg shadow border">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Informações da Venda</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Cliente *
              </label>
              <input
                type="text"
                {...register('customer')}
                className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                  errors.customer ? 'border-red-300' : 'border-gray-300'
                }`}
                placeholder="Nome do cliente"
              />
              {errors.customer && (
                <p className="mt-1 text-sm text-red-600">{errors.customer.message}</p>
              )}
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Filial *
              </label>
              <input
                type="text"
                {...register('branch')}
                className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                  errors.branch ? 'border-red-300' : 'border-gray-300'
                }`}
                placeholder="Nome da filial"
              />
              {errors.branch && (
                <p className="mt-1 text-sm text-red-600">{errors.branch.message}</p>
              )}
            </div>
          </div>
        </div>

        {/* Items */}
        <div className="bg-white p-6 rounded-lg shadow border">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-gray-900">Itens da Venda</h2>
            <button
              type="button"
              onClick={() => append({ product: '', quantity: 1, unitPrice: 0, discount: 0 })}
              className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700"
            >
              <Plus className="h-4 w-4 mr-1" />
              Adicionar Item
            </button>
          </div>

          {fields.map((field, index) => {
            const item = watchedItems?.[index];
            const discountInfo = getDiscountInfo(item?.quantity || 0);
            const itemTotal = calculateItemTotal(item || { product: '', quantity: 0, unitPrice: 0, discount: 0 });

            return (
              <div key={field.id} className="border border-gray-200 rounded-lg p-4 mb-4">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-md font-medium text-gray-900">Item {index + 1}</h3>
                  {fields.length > 1 && (
                    <button
                      type="button"
                      onClick={() => remove(index)}
                      className="inline-flex items-center px-2 py-1 border border-red-300 text-sm font-medium rounded-md text-red-700 bg-white hover:bg-red-50"
                    >
                      <Trash2 className="h-4 w-4 mr-1" />
                      Remover
                    </button>
                  )}
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Produto *
                    </label>
                    <input
                      type="text"
                      {...register(`items.${index}.product`)}
                      className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                        errors.items?.[index]?.product ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="Nome do produto"
                    />
                    {errors.items?.[index]?.product && (
                      <p className="mt-1 text-sm text-red-600">{errors.items[index]?.product?.message}</p>
                    )}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Quantidade *
                    </label>
                    <input
                      type="number"
                      {...register(`items.${index}.quantity`, { valueAsNumber: true })}
                      className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                        errors.items?.[index]?.quantity ? 'border-red-300' : 'border-gray-300'
                      }`}
                      min="1"
                      max="19"
                    />
                    {errors.items?.[index]?.quantity && (
                      <p className="mt-1 text-sm text-red-600">{errors.items[index]?.quantity?.message}</p>
                    )}
                    <p className={`mt-1 text-xs ${discountInfo.color}`}>
                      {discountInfo.text}
                    </p>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Preço Unitário *
                    </label>
                    <input
                      type="number"
                      step="0.01"
                      {...register(`items.${index}.unitPrice`, { valueAsNumber: true })}
                      className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                        errors.items?.[index]?.unitPrice ? 'border-red-300' : 'border-gray-300'
                      }`}
                      min="0.01"
                    />
                    {errors.items?.[index]?.unitPrice && (
                      <p className="mt-1 text-sm text-red-600">{errors.items[index]?.unitPrice?.message}</p>
                    )}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Desconto (%)
                    </label>
                    <input
                      type="number"
                      step="0.01"
                      {...register(`items.${index}.discount`, { valueAsNumber: true })}
                      className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                        errors.items?.[index]?.discount ? 'border-red-300' : 'border-gray-300'
                      }`}
                      min="0"
                      max="100"
                    />
                    {errors.items?.[index]?.discount && (
                      <p className="mt-1 text-sm text-red-600">{errors.items[index]?.discount?.message}</p>
                    )}
                  </div>
                </div>

                <div className="mt-4 p-3 bg-gray-50 rounded-md">
                  <div className="flex justify-between items-center">
                    <span className="text-sm font-medium text-gray-700">Total do Item:</span>
                    <span className="text-lg font-semibold text-gray-900">
                      {formatCurrency(itemTotal)}
                    </span>
                  </div>
                </div>
              </div>
            );
          })}

          {errors.items && (
            <p className="mt-2 text-sm text-red-600">{errors.items.message}</p>
          )}
        </div>

        {/* Total */}
        <div className="bg-white p-6 rounded-lg shadow border">
          <div className="flex justify-between items-center">
            <h2 className="text-xl font-semibold text-gray-900">Total da Venda</h2>
            <div className="text-right">
              <p className="text-3xl font-bold text-gray-900">
                {formatCurrency(calculateTotal())}
              </p>
                             <p className="text-sm text-gray-500">
                 {(watchedItems || []).length} item{(watchedItems || []).length !== 1 ? 's' : ''}
               </p>
            </div>
          </div>
        </div>

        {/* Submit Button */}
        <div className="flex justify-end space-x-4">
          <button
            type="button"
            onClick={() => navigate('/sales')}
            className="px-6 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 bg-white hover:bg-gray-50"
          >
            Cancelar
          </button>
          <button
            type="submit"
            disabled={loading}
            className="inline-flex items-center px-6 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <Save className="h-4 w-4 mr-1" />
            {loading ? 'Criando...' : 'Criar Venda'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateSale;

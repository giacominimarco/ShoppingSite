import axios, { AxiosResponse } from 'axios';
import {
  Sale,
  CreateSaleRequest,
  CreateSaleResponse,
  GetSaleResponse,
  GetSalesRequest,
  GetSalesResponse,
  CancelSaleResponse,
  ApiError
} from '../types/api';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7001/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para tratar erros
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.data) {
      return Promise.reject(error.response.data);
    }
    return Promise.reject({
      success: false,
      message: 'Erro de conexão com o servidor'
    });
  }
);

export const salesApi = {
  // Criar uma nova venda
  createSale: async (saleData: CreateSaleRequest): Promise<CreateSaleResponse> => {
    const response: AxiosResponse<CreateSaleResponse> = await api.post('/sales', saleData);
    return response.data;
  },

  // Obter uma venda específica
  getSale: async (id: string): Promise<GetSaleResponse> => {
    const response: AxiosResponse<GetSaleResponse> = await api.get(`/sales/${id}`);
    return response.data;
  },

  // Listar vendas com filtros e paginação
  getSales: async (params: GetSalesRequest = {}): Promise<GetSalesResponse> => {
    const response: AxiosResponse<GetSalesResponse> = await api.get('/sales', { params });
    return response.data;
  },

  // Cancelar uma venda
  cancelSale: async (id: string): Promise<CancelSaleResponse> => {
    const response: AxiosResponse<CancelSaleResponse> = await api.post(`/sales/${id}/cancel`);
    return response.data;
  },
};

export default api;

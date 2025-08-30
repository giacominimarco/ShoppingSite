import axios, { AxiosResponse } from 'axios';
import {
  CreateSaleRequest,
  GetSalesRequest,
  GetSalesResponse,
  Sale,
} from '../types/api';

// Detecta o protocolo atual e define a porta correta do backend
const protocol = window.location.protocol; // 'http:' ou 'https:'
const port = protocol === 'https:' ? 7181 : 5119;

const API_BASE_URL = process.env.REACT_APP_API_URL || `${protocol}//localhost:${port}/api`;

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
  createSale: async (saleData: CreateSaleRequest): Promise<Sale> => {
    const response: AxiosResponse<Sale> = await api.post('/sales', saleData);
    return response.data;
  },

  // Obter uma venda específica
  getSale: async (id: string): Promise<Sale> => {
    const response: AxiosResponse<Sale> = await api.get(`/sales/${id}`);
    return response.data;
  },

  // Listar vendas com filtros e paginação
  getSales: async (params: GetSalesRequest = {}): Promise<GetSalesResponse> => {
    const response: AxiosResponse<GetSalesResponse> = await api.get('/sales', { params });
    return response.data;
  },

  // Cancelar uma venda
  cancelSale: async (id: string): Promise<Sale> => {
    const response: AxiosResponse<Sale> = await api.post(`/sales/${id}/cancel`);
    return response.data;
  },
};

export default api;
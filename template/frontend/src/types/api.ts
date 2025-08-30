export interface SaleItem {
  id: string;
  saleId: string;
  product: string;
  quantity: number;
  unitPrice: number;
  discount: number;
  totalAmount: number;
}

export interface Sale {
  id: string;
  saleNumber: string;
  saleDate: string;
  customer: string;
  totalAmount: number;
  branch: string;
  items: SaleItem[];
  status: 'Active' | 'Cancelled';
  createdAt: string;
  updatedAt?: string;
}

export interface CreateSaleItemRequest {
  product: string;
  quantity: number;
  unitPrice: number;
  discount?: number;
}

export interface CreateSaleRequest {
  customer: string;
  branch: string;
  items: CreateSaleItemRequest[];
}

export interface CreateSaleResponse {
  success: boolean;
  data: Sale;
  message?: string;
}

export interface GetSaleResponse {
  success: boolean;
  data: Sale;
  message?: string;
}

export interface GetSalesRequest {
  page?: number;
  size?: number;
  customer?: string;
  branch?: string;
  status?: string;
  startDate?: string;
  endDate?: string;
}

export interface GetSalesResponse {
  sales: Sale[];
  totalCount: number;
  page: number;
  size: number;
  totalPages: number;
}

export interface CancelSaleResponse {
  success: boolean;
  data: Sale;
  message?: string;
}

export interface ApiError {
  success: false;
  message: string;
  errors?: Record<string, string[]>;
}

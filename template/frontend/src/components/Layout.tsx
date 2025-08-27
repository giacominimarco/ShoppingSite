import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import { ShoppingCart, List, Plus, Home } from 'lucide-react';

interface LayoutProps {
  children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center">
              <ShoppingCart className="h-8 w-8 text-blue-600 mr-3" />
              <h1 className="text-xl font-semibold text-gray-900">
                Sistema de Vendas
              </h1>
            </div>
            <nav className="flex space-x-8">
              <Link
                to="/"
                className="flex items-center text-gray-500 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium"
              >
                <Home className="h-4 w-4 mr-1" />
                Início
              </Link>
              <Link
                to="/sales"
                className="flex items-center text-gray-500 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium"
              >
                <List className="h-4 w-4 mr-1" />
                Vendas
              </Link>
              <Link
                to="/sales/new"
                className="flex items-center text-gray-500 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium"
              >
                <Plus className="h-4 w-4 mr-1" />
                Nova Venda
              </Link>
            </nav>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        {children || <Outlet />}
      </main>
    </div>
  );
};

export default Layout;

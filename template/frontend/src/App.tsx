import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Home from './pages/Home';
import SalesList from './pages/SalesList';
import SaleDetail from './pages/SaleDetail';
import CreateSale from './pages/CreateSale';
import './App.css';

function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/sales" element={<SalesList />} />
          <Route path="/sales/new" element={<CreateSale />} />
          <Route path="/sales/:id" element={<SaleDetail />} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;

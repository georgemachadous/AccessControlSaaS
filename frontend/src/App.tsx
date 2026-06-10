import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './hooks/useAuth';
import { ToastProvider } from './components/ToastContainer';
import Login from './pages/Login';
import Layout from './pages/Layout';
import Dashboard from './pages/Dashboard';
import CrudPage from './pages/CrudPage';

function App() {
  const { isAuthenticated, loading } = useAuth();

  if (loading) return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', background: 'var(--bg-primary)' }}>
      <div style={{ textAlign: 'center' }}>
        <div className="skeleton" style={{ width: 48, height: 48, borderRadius: 'var(--radius-lg)', margin: '0 auto var(--space-lg)' }} />
        <div className="skeleton" style={{ width: 120, height: 16, margin: '0 auto' }} />
      </div>
    </div>
  );

  return (
    <ToastProvider>
      <Routes>
        <Route path="/login" element={isAuthenticated ? <Navigate to="/" /> : <Login />} />
        <Route path="/" element={isAuthenticated ? <Layout /> : <Navigate to="/login" />}>
          <Route index element={<Dashboard />} />
          <Route path="companies" element={<CrudPage type="companies" title="Empresas" />} />
          <Route path="branches" element={<CrudPage type="branches" title="Filiais" />} />
          <Route path="applications" element={<CrudPage type="applications" title="Aplicacoes" />} />
          <Route path="functionalities" element={<CrudPage type="functionalities" title="Funcionalidades" />} />
          <Route path="roles" element={<CrudPage type="roles" title="Perfis" />} />
          <Route path="rolefunctionalities" element={<CrudPage type="rolefunctionalities" title="Permissoes de Perfil" />} />
          <Route path="users" element={<CrudPage type="users" title="Usuarios" />} />
          <Route path="userapplicationroles" element={<CrudPage type="userapplicationroles" title="Perfis por Aplicacao" />} />
          <Route path="permissions" element={<CrudPage type="permissions" title="Permissoes" />} />
        </Route>
      </Routes>
    </ToastProvider>
  );
}

export default App;

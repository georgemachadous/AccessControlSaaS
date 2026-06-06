import { Routes, Route } from 'react-router-dom'
import { Toaster } from '@/components/ui/Toast'
import { AuthProvider } from '@/hooks/useAuth'
import { ThemeProvider } from '@/hooks/useTheme'
import Layout from '@/components/layout/Layout'
import LoginPage from '@/pages/LoginPage'
import DashboardPage from '@/pages/DashboardPage'
import EmpresasPage from '@/pages/EmpresasPage'
import UsuariosPage from '@/pages/UsuariosPage'
import PerfisPage from '@/pages/PerfisPage'
import AplicacoesPage from '@/pages/AplicacoesPage'
import AuditoriaPage from '@/pages/AuditoriaPage'
import NotFoundPage from '@/pages/NotFoundPage'
import ProtectedRoute from '@/components/layout/ProtectedRoute'

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <Toaster />
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/" element={
            <ProtectedRoute>
              <Layout />
            </ProtectedRoute>
          }>
            <Route index element={<DashboardPage />} />
            <Route path="empresas" element={<EmpresasPage />} />
            <Route path="usuarios" element={<UsuariosPage />} />
            <Route path="perfis" element={<PerfisPage />} />
            <Route path="aplicacoes" element={<AplicacoesPage />} />
            <Route path="auditoria" element={<AuditoriaPage />} />
          </Route>
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </AuthProvider>
    </ThemeProvider>
  )
}

export default App

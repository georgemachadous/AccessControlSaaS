import { createContext, useEffect, type ReactNode } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from '@/stores/authStore'

const AuthContext = createContext<null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const navigate = useNavigate()
  const { isAuthenticated, token } = useAuthStore()

  useEffect(() => {
    if (!isAuthenticated && !token && window.location.pathname !== '/login') {
      navigate('/login')
    }
  }, [isAuthenticated, token, navigate])

  return <AuthContext.Provider value={null}>{children}</AuthContext.Provider>
}

export function useAuth() {
  return useAuthStore()
}

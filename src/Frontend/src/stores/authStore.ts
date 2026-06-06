import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { Usuario } from '@/types/api'

interface AuthState {
  token: string | null
  refreshToken: string | null
  user: Usuario | null
  isAuthenticated: boolean
  setAuth: (token: string, refreshToken: string, user: Usuario) => void
  clearAuth: () => void
  setUser: (user: Usuario) => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      refreshToken: null,
      user: null,
      isAuthenticated: false,
      setAuth: (token, refreshToken, user) =>
        set({ token, refreshToken, user, isAuthenticated: true }),
      clearAuth: () =>
        set({ token: null, refreshToken: null, user: null, isAuthenticated: false }),
      setUser: (user) => set({ user })
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({ token: state.token, refreshToken: state.refreshToken, user: state.user })
    }
  )
)

import axios from 'axios'
import { useAuthStore } from '@/stores/authStore'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  headers: {
    'Content-Type': 'application/json'
  },
  timeout: 30000
})

api.interceptors.request.use(
  (config) => {
    const token = useAuthStore.getState().token
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    const correlationId = crypto.randomUUID()
    config.headers['X-Correlation-ID'] = correlationId
    return config
  },
  (error) => Promise.reject(error)
)

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true
      try {
        const refreshToken = useAuthStore.getState().refreshToken
        if (refreshToken) {
          const response = await axios.post('/api/auth/refresh', { refreshToken })
          const { accessToken, refreshToken: newRefreshToken } = response.data
          useAuthStore.getState().setAuth(accessToken, newRefreshToken, useAuthStore.getState().user!)
          originalRequest.headers.Authorization = `Bearer ${accessToken}`
          return api(originalRequest)
        }
      } catch {
        useAuthStore.getState().clearAuth()
        window.location.href = '/login'
      }
    }
    return Promise.reject(error)
  }
)

export default api

export const authApi = {
  login: (email: string, senha: string) => api.post('/auth/login', { email, senha }),
  logout: () => api.post('/auth/logout'),
  refresh: (refreshToken: string) => api.post('/auth/refresh', { refreshToken }),
  me: () => api.get('/auth/me')
}

export const empresaApi = {
  list: (params?: { page?: number; pageSize?: number; search?: string }) =>
    api.get('/empresas', { params }),
  get: (id: string) => api.get(`/empresas/${id}`),
  create: (data: unknown) => api.post('/empresas', data),
  update: (id: string, data: unknown) => api.put(`/empresas/${id}`, data),
  delete: (id: string) => api.delete(`/empresas/${id}`),
  uploadLogo: (id: string, file: File) => {
    const formData = new FormData()
    formData.append('file', file)
    return api.post(`/empresas/${id}/logo`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  }
}

export const usuarioApi = {
  list: (params?: { page?: number; pageSize?: number; search?: string }) =>
    api.get('/usuarios', { params }),
  get: (id: string) => api.get(`/usuarios/${id}`),
  create: (data: unknown) => api.post('/usuarios', data),
  update: (id: string, data: unknown) => api.put(`/usuarios/${id}`, data),
  delete: (id: string) => api.delete(`/usuarios/${id}`),
  activate: (id: string) => api.post(`/usuarios/${id}/ativar`),
  deactivate: (id: string) => api.post(`/usuarios/${id}/desativar`)
}

export const perfilApi = {
  list: (empresaId: string, params?: { page?: number; pageSize?: number }) =>
    api.get('/perfis', { params: { ...params, empresaId } }),
  get: (id: string) => api.get(`/perfis/${id}`),
  create: (data: unknown) => api.post('/perfis', data),
  update: (id: string, data: unknown) => api.put(`/perfis/${id}`, data),
  delete: (id: string) => api.delete(`/perfis/${id}`),
  assignFuncionalidade: (perfilId: string, data: unknown) =>
    api.post(`/perfis/${perfilId}/funcionalidades`, data),
  removeFuncionalidade: (perfilId: string, funcionalidadeId: string) =>
    api.delete(`/perfis/${perfilId}/funcionalidades/${funcionalidadeId}`)
}

export const aplicacaoApi = {
  list: (params?: { page?: number; pageSize?: number; search?: string }) =>
    api.get('/aplicacoes', { params }),
  get: (id: string) => api.get(`/aplicacoes/${id}`),
  create: (data: unknown) => api.post('/aplicacoes', data),
  update: (id: string, data: unknown) => api.put(`/aplicacoes/${id}`, data),
  delete: (id: string) => api.delete(`/aplicacoes/${id}`)
}

export const dashboardApi = {
  stats: () => api.get('/dashboard/stats'),
  loginsPorDia: (dias?: number) => api.get('/dashboard/logins-por-dia', { params: { dias } })
}

export const auditoriaApi = {
  list: (params?: { page?: number; pageSize?: number; entidade?: string; acao?: string }) =>
    api.get('/auditoria', { params })
}

export const notificacaoApi = {
  list: (params?: { page?: number; pageSize?: number; lida?: boolean }) =>
    api.get('/notificacoes', { params }),
  unreadCount: () => api.get('/notificacoes/nao-lidas/count'),
  markAsRead: (id: string) => api.post(`/notificacoes/${id}/ler`)
}

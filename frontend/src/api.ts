import axios from 'axios';
import type { AuthResponse } from './types';

const api = axios.create({ baseURL: '/api' });

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;

export const authApi = {
  login: (email: string, password: string) => api.post<AuthResponse>('/auth/login', { email, password }),
  ssoLogin: (provider: string, token: string) => api.post<AuthResponse>('/auth/sso', { provider, token }),
};

export const crudApi = (type: string) => ({
  getAll: () => api.get(`/${type}`),
  getById: (id: string) => api.get(`/${type}/${id}`),
  create: (data: unknown) => api.post(`/${type}`, data),
  update: (id: string, data: unknown) => api.put(`/${type}/${id}`, data),
  delete: (id: string) => api.delete(`/${type}/${id}`),
});

export const dashboardApi = {
  summary: () => api.get('/dashboard/summary'),
  activity: (count = 10) => api.get(`/dashboard/activity?count=${count}`),
};

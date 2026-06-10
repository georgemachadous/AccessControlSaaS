import { useState, useEffect, useCallback } from 'react';
import type { AuthResponse, User, UserAppClaim } from '../types';

export function useAuth() {
  const [user, setUser] = useState<User | null>(null);
  const [claims, setClaims] = useState<UserAppClaim[]>([]);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const stored = localStorage.getItem('user');
    const token = localStorage.getItem('token');
    if (stored && token) {
      setUser(JSON.parse(stored));
      const c = localStorage.getItem('claims');
      if (c) setClaims(JSON.parse(c));
      setIsAuthenticated(true);
    }
    setLoading(false);
  }, []);

  const login = useCallback((response: AuthResponse) => {
    localStorage.setItem('token', response.token);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.user));
    localStorage.setItem('claims', JSON.stringify(response.claims));
    setUser(response.user);
    setClaims(response.claims);
    setIsAuthenticated(true);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    localStorage.removeItem('claims');
    setUser(null);
    setClaims([]);
    setIsAuthenticated(false);
  }, []);

  const hasPermission = useCallback((appId: string, funcCode: string, action: 'C' | 'R' | 'U' | 'D') => {
    if (user?.isSuperAdmin) return true;
    const claim = claims.find(c => c.applicationId === appId);
    if (!claim) return false;
    const perm = claim.permissions.find(p => p.startsWith(`${funcCode}:`));
    if (!perm) return false;
    return perm.includes(action);
  }, [user, claims]);

  return { user, claims, isAuthenticated, loading, login, logout, hasPermission };
}

import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import {
  DashboardIcon, BuildingIcon, BranchIcon, AppIcon, FunctionIcon,
  RoleIcon, ShieldIcon, UserIcon, LinkIcon, PermissionIcon, LogIcon,
  LogoutIcon, MenuIcon, ChevronRightIcon
} from './Icons';

interface MenuItem {
  path: string;
  label: string;
  icon: React.ReactNode;
  section?: string;
}

const menuItems: MenuItem[] = [
  { path: '/', label: 'Dashboard', icon: <DashboardIcon size={20} />, section: 'Principal' },
  { path: '/companies', label: 'Empresas', icon: <BuildingIcon size={20} />, section: 'Cadastros' },
  { path: '/branches', label: 'Filiais', icon: <BranchIcon size={20} />, section: 'Cadastros' },
  { path: '/applications', label: 'Aplicacoes', icon: <AppIcon size={20} />, section: 'Cadastros' },
  { path: '/functionalities', label: 'Funcionalidades', icon: <FunctionIcon size={20} />, section: 'Cadastros' },
  { path: '/roles', label: 'Perfis', icon: <RoleIcon size={20} />, section: 'Permissoes' },
  { path: '/rolefunctionalities', label: 'Perm. Perfil', icon: <ShieldIcon size={20} />, section: 'Permissoes' },
  { path: '/users', label: 'Usuarios', icon: <UserIcon size={20} />, section: 'Usuarios' },
  { path: '/userapplicationroles', label: 'User App Role', icon: <LinkIcon size={20} />, section: 'Usuarios' },
  { path: '/permissions', label: 'Permissoes', icon: <PermissionIcon size={20} />, section: 'Usuarios' },
];

export default function Sidebar() {
  const location = useLocation();
  const { user, logout } = useAuth();
  const [collapsed, setCollapsed] = useState(false);

  const grouped = menuItems.reduce((acc, item) => {
    const section = item.section || 'Outros';
    if (!acc[section]) acc[section] = [];
    acc[section].push(item);
    return acc;
  }, {} as Record<string, MenuItem[]>);

  return (
    <aside className={`sidebar ${collapsed ? 'sidebar-collapsed' : ''}`}>
      <div className="sidebar-header">
        <div className="sidebar-logo">AC</div>
        {!collapsed && <span style={{ fontWeight: 700, fontSize: 'var(--font-md)', color: 'var(--text-primary)' }}>AccessControl</span>}
        <button
          onClick={() => setCollapsed(!collapsed)}
          className="header-btn"
          style={{ marginLeft: 'auto' }}
          title={collapsed ? 'Expandir' : 'Recolher'}
        >
          <MenuIcon size={18} />
        </button>
      </div>

      <nav className="sidebar-nav">
        {Object.entries(grouped).map(([section, items]) => (
          <div key={section} className="sidebar-section">
            {!collapsed && <div className="sidebar-section-title">{section}</div>}
            {items.map(item => {
              const isActive = location.pathname === item.path;
              return (
                <Link
                  key={item.path}
                  to={item.path}
                  className={`sidebar-item ${isActive ? 'active' : ''}`}
                  title={collapsed ? item.label : undefined}
                >
                  <span className="sidebar-icon">{item.icon}</span>
                  {!collapsed && <span className="truncate">{item.label}</span>}
                  {isActive && !collapsed && <ChevronRightIcon size={14} className="sidebar-chevron" />}
                </Link>
              );
            })}
          </div>
        ))}
      </nav>

      <div className="sidebar-footer">
        <div className="sidebar-user">
          <div className="sidebar-avatar">{user?.name?.charAt(0).toUpperCase() || 'U'}</div>
          {!collapsed && (
            <div style={{ minWidth: 0 }}>
              <div className="truncate" style={{ fontSize: 'var(--font-sm)', fontWeight: 600, color: 'var(--text-primary)' }}>{user?.name}</div>
              <div className="truncate" style={{ fontSize: 'var(--font-xs)', color: 'var(--text-muted)' }}>{user?.email}</div>
            </div>
          )}
        </div>
        <button onClick={logout} className="sidebar-item" style={{ marginTop: 'var(--space-sm)', color: 'var(--danger)' }}>
          <span className="sidebar-icon"><LogoutIcon size={18} /></span>
          {!collapsed && <span>Sair</span>}
        </button>
      </div>
    </aside>
  );
}

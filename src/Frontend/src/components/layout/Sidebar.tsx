import { NavLink, useLocation } from 'react-router-dom'
import { useState } from 'react'
import {
  LayoutDashboard, Building2, Users, Shield, AppWindow, FileText, Bell, ChevronLeft, ChevronRight, LogOut
} from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'

const navItems = [
  { path: '/', icon: LayoutDashboard, label: 'Dashboard' },
  { path: '/empresas', icon: Building2, label: 'Empresas' },
  { path: '/usuarios', icon: Users, label: 'Usuarios' },
  { path: '/perfis', icon: Shield, label: 'Perfis' },
  { path: '/aplicacoes', icon: AppWindow, label: 'Aplicacoes' },
  { path: '/auditoria', icon: FileText, label: 'Auditoria' },
]

export default function Sidebar() {
  const [collapsed, setCollapsed] = useState(false)
  const location = useLocation()
  const { clearAuth } = useAuth()

  return (
    <aside className={`${collapsed ? 'w-16' : 'w-64'} bg-slate-900 border-r border-slate-800 transition-all duration-300 flex flex-col`}>
      <div className="h-16 flex items-center justify-between px-4 border-b border-slate-800">
        {!collapsed && (
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-primary-600 flex items-center justify-center">
              <Shield className="w-5 h-5 text-white" />
            </div>
            <span className="font-bold text-lg">AccessControl</span>
          </div>
        )}
        <button
          onClick={() => setCollapsed(!collapsed)}
          className="p-1 rounded-md hover:bg-slate-800 transition-colors"
        >
          {collapsed ? <ChevronRight className="w-5 h-5" /> : <ChevronLeft className="w-5 h-5" />}
        </button>
      </div>

      <nav className="flex-1 py-4 px-2 space-y-1">
        {navItems.map((item) => {
          const Icon = item.icon
          const isActive = location.pathname === item.path
          return (
            <NavLink
              key={item.path}
              to={item.path}
              className={`flex items-center gap-3 px-3 py-2.5 rounded-lg transition-all duration-200 ${
                isActive
                  ? 'bg-primary-600/20 text-primary-400 border-r-2 border-primary-500'
                  : 'text-slate-400 hover:bg-slate-800 hover:text-slate-200'
              }`}
              title={collapsed ? item.label : undefined}
            >
              <Icon className="w-5 h-5 flex-shrink-0" />
              {!collapsed && <span className="font-medium text-sm">{item.label}</span>}
            </NavLink>
          )
        })}
      </nav>

      <div className="p-2 border-t border-slate-800">
        <button
          onClick={clearAuth}
          className="flex items-center gap-3 px-3 py-2.5 rounded-lg text-slate-400 hover:bg-slate-800 hover:text-red-400 transition-all w-full"
          title={collapsed ? 'Sair' : undefined}
        >
          <LogOut className="w-5 h-5 flex-shrink-0" />
          {!collapsed && <span className="font-medium text-sm">Sair</span>}
        </button>
      </div>
    </aside>
  )
}

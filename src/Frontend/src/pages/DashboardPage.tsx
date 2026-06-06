import { useQuery } from '@tanstack/react-query'
import { Building2, Users, AppWindow, Shield, Activity, TrendingUp, Clock } from 'lucide-react'
import { dashboardApi } from '@/services/api'
import type { DashboardStats } from '@/types/api'

function StatCard({ title, value, icon: Icon, color, trend }: {
  title: string
  value: number
  icon: React.ElementType
  color: string
  trend?: number
}) {
  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl p-6 hover:border-slate-700 transition-colors">
      <div className="flex items-center justify-between mb-4">
        <div className={`w-12 h-12 rounded-lg ${color} flex items-center justify-center`}>
          <Icon className="w-6 h-6 text-white" />
        </div>
        {trend !== undefined && (
          <span className={`text-sm font-medium ${trend >= 0 ? 'text-emerald-400' : 'text-red-400'}`}>
            {trend >= 0 ? '+' : ''}{trend}%
          </span>
        )}
      </div>
      <h3 className="text-2xl font-bold text-white">{value.toLocaleString()}</h3>
      <p className="text-slate-400 text-sm mt-1">{title}</p>
    </div>
  )
}

export default function DashboardPage() {
  const { data: stats, isLoading } = useQuery<DashboardStats>({
    queryKey: ['dashboard-stats'],
    queryFn: () => dashboardApi.stats().then(r => r.data)
  })

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {[...Array(4)].map((_, i) => (
          <div key={i} className="bg-slate-900 border border-slate-800 rounded-xl p-6 animate-pulse">
            <div className="w-12 h-12 bg-slate-800 rounded-lg mb-4"></div>
            <div className="h-8 bg-slate-800 rounded mb-2"></div>
            <div className="h-4 bg-slate-800 rounded w-2/3"></div>
          </div>
        ))}
      </div>
    )
  }

  const data = stats || {
    totalEmpresas: 0, totalUsuarios: 0, totalAplicacoes: 0, totalPerfis: 0,
    loginsHoje: 0, usuariosAtivos: 0, sessoesAtivas: 0, loginsPorDia: []
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-white">Dashboard</h1>
        <p className="text-slate-400 mt-1">Visao geral do sistema</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard title="Empresas" value={data.totalEmpresas} icon={Building2} color="bg-blue-600" trend={5.2} />
        <StatCard title="Usuarios" value={data.totalUsuarios} icon={Users} color="bg-emerald-600" trend={12.8} />
        <StatCard title="Aplicacoes" value={data.totalAplicacoes} icon={AppWindow} color="bg-amber-600" trend={-2.1} />
        <StatCard title="Perfis" value={data.totalPerfis} icon={Shield} color="bg-rose-600" trend={8.4} />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 bg-slate-900 border border-slate-800 rounded-xl p-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-white">Logins por Dia</h2>
            <Activity className="w-5 h-5 text-slate-500" />
          </div>
          <div className="h-64 flex items-end gap-2">
            {data.loginsPorDia.map((day, i) => {
              const max = Math.max(...data.loginsPorDia.map(d => d.quantidade), 1)
              const height = (day.quantidade / max) * 100
              return (
                <div key={i} className="flex-1 flex flex-col items-center gap-1">
                  <div
                    className="w-full bg-primary-600/60 rounded-t hover:bg-primary-500/80 transition-colors"
                    style={{ height: `${height}%` }}
                    title={`${day.data}: ${day.quantidade} logins`}
                  ></div>
                  <span className="text-xs text-slate-500">
                    {new Date(day.data).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' })}
                  </span>
                </div>
              )
            })}
          </div>
        </div>

        <div className="bg-slate-900 border border-slate-800 rounded-xl p-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-white">Atividade</h2>
            <Clock className="w-5 h-5 text-slate-500" />
          </div>
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <span className="text-slate-400">Logins Hoje</span>
              <span className="text-white font-semibold">{data.loginsHoje}</span>
            </div>
            <div className="flex items-center justify-between">
              <span className="text-slate-400">Usuarios Ativos</span>
              <span className="text-white font-semibold">{data.usuariosAtivos}</span>
            </div>
            <div className="flex items-center justify-between">
              <span className="text-slate-400">Sessoes Ativas</span>
              <span className="text-white font-semibold">{data.sessoesAtivas}</span>
            </div>
            <div className="h-px bg-slate-800"></div>
            <div className="flex items-center justify-between">
              <span className="text-slate-400">Taxa de Atividade</span>
              <div className="flex items-center gap-2">
                <TrendingUp className="w-4 h-4 text-emerald-400" />
                <span className="text-emerald-400 font-semibold">
                  {data.totalUsuarios > 0 ? Math.round((data.usuariosAtivos / data.totalUsuarios) * 100) : 0}%
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { FileText, Search, Filter, Loader2, Calendar, ArrowRightLeft, User, Globe } from 'lucide-react'
import { auditoriaApi } from '@/services/api'
import type { LogAuditoria, PaginatedResult } from '@/types/api'

const acaoColors: Record<string, string> = {
  CREATE: 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20',
  UPDATE: 'bg-blue-500/10 text-blue-400 border-blue-500/20',
  DELETE: 'bg-red-500/10 text-red-400 border-red-500/20',
  LOGIN: 'bg-primary-500/10 text-primary-400 border-primary-500/20',
  LOGOUT: 'bg-slate-500/10 text-slate-400 border-slate-500/20',
  ACTIVATE: 'bg-amber-500/10 text-amber-400 border-amber-500/20',
  DEACTIVATE: 'bg-orange-500/10 text-orange-400 border-orange-500/20',
  CHANGE_PASSWORD: 'bg-purple-500/10 text-purple-400 border-purple-500/20',
  ASSIGN: 'bg-cyan-500/10 text-cyan-400 border-cyan-500/20',
  REMOVE: 'bg-pink-500/10 text-pink-400 border-pink-500/20'
}

export default function AuditoriaPage() {
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [entidadeFilter, setEntidadeFilter] = useState('')
  const [acaoFilter, setAcaoFilter] = useState('')

  const { data, isLoading } = useQuery<PaginatedResult<LogAuditoria>>({
    queryKey: ['auditoria', page, search, entidadeFilter, acaoFilter],
    queryFn: () => auditoriaApi.list({ page, pageSize: 20, entidade: entidadeFilter || undefined, acao: acaoFilter || undefined }).then(r => r.data)
  })

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-white">Auditoria</h1>
        <p className="text-slate-400 mt-1">Registro de todas as operacoes do sistema</p>
      </div>

      <div className="flex flex-wrap items-center gap-4">
        <div className="relative flex-1 min-w-[200px] max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500" />
          <input
            type="text"
            placeholder="Buscar registros..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full bg-slate-900 border border-slate-800 rounded-lg pl-10 pr-4 py-2.5 text-sm text-white placeholder-slate-500 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>
        <div className="flex items-center gap-2">
          <Filter className="w-4 h-4 text-slate-500" />
          <select
            value={entidadeFilter}
            onChange={(e) => setEntidadeFilter(e.target.value)}
            className="bg-slate-900 border border-slate-800 rounded-lg px-3 py-2.5 text-sm text-white focus:outline-none focus:ring-2 focus:ring-primary-500"
          >
            <option value="">Todas Entidades</option>
            <option value="Empresa">Empresa</option>
            <option value="Usuario">Usuario</option>
            <option value="Perfil">Perfil</option>
            <option value="Aplicacao">Aplicacao</option>
          </select>
          <select
            value={acaoFilter}
            onChange={(e) => setAcaoFilter(e.target.value)}
            className="bg-slate-900 border border-slate-800 rounded-lg px-3 py-2.5 text-sm text-white focus:outline-none focus:ring-2 focus:ring-primary-500"
          >
            <option value="">Todas Acoes</option>
            <option value="CREATE">Criar</option>
            <option value="UPDATE">Atualizar</option>
            <option value="DELETE">Excluir</option>
            <option value="LOGIN">Login</option>
            <option value="LOGOUT">Logout</option>
          </select>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead>
              <tr className="border-b border-slate-800">
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Data</th>
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Acao</th>
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Entidade</th>
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Usuario</th>
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">IP</th>
                <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Observacao</th>
              </tr>
            </thead>
            <tbody>
              {isLoading ? (
                <tr><td colSpan={6} className="px-6 py-12 text-center"><Loader2 className="w-6 h-6 animate-spin mx-auto text-primary-500" /></td></tr>
              ) : data?.items.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-12 text-center text-slate-500">Nenhum registro encontrado</td></tr>
              ) : (
                data?.items.map((log) => (
                  <tr key={log.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2 text-slate-300">
                        <Calendar className="w-4 h-4 text-slate-500" />
                        <span className="text-sm">{new Date(log.createdAt).toLocaleString('pt-BR')}</span>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <span className={`inline-flex px-2.5 py-1 rounded-full text-xs font-medium border ${acaoColors[log.acao] || 'bg-slate-500/10 text-slate-400 border-slate-500/20'}`}>
                        {log.acao}
                      </span>
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2">
                        <ArrowRightLeft className="w-4 h-4 text-slate-500" />
                        <span className="text-slate-300 text-sm">{log.entidade}</span>
                        <span className="text-slate-500 text-xs">({log.entidadeId.slice(0, 8)}...)</span>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2 text-slate-300">
                        <User className="w-4 h-4 text-slate-500" />
                        <span className="text-sm">{log.createdBy}</span>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2 text-slate-300">
                        <Globe className="w-4 h-4 text-slate-500" />
                        <span className="text-sm font-mono">{log.ipAddress}</span>
                      </div>
                    </td>
                    <td className="px-6 py-4 text-slate-300 text-sm max-w-xs truncate">
                      {log.observacao || '-'}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>

        {data && data.totalPages > 1 && (
          <div className="flex items-center justify-between px-6 py-4 border-t border-slate-800">
            <p className="text-sm text-slate-500">Mostrando {data.items.length} de {data.totalCount} registros</p>
            <div className="flex items-center gap-2">
              <button onClick={() => setPage(p => Math.max(1, p - 1))} disabled={page === 1} className="px-3 py-1.5 rounded-lg bg-slate-800 text-slate-300 hover:bg-slate-700 disabled:opacity-50 text-sm">Anterior</button>
              <span className="text-sm text-slate-400">Pagina {page} de {data.totalPages}</span>
              <button onClick={() => setPage(p => Math.min(data.totalPages, p + 1))} disabled={page === data.totalPages} className="px-3 py-1.5 rounded-lg bg-slate-800 text-slate-300 hover:bg-slate-700 disabled:opacity-50 text-sm">Proxima</button>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}

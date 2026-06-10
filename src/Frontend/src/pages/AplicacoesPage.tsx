import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { AppWindow, Plus, Search, Edit2, Trash2, ExternalLink, Globe } from 'lucide-react'
import { aplicacaoApi } from '@/services/api'
import type { Aplicacao, PaginatedResult } from '@/types/api'

export default function AplicacoesPage() {
  const [search, setSearch] = useState('')
  const [page] = useState(1)

  const { data, isLoading } = useQuery<PaginatedResult<Aplicacao>>({
    queryKey: ['aplicacoes', page, search],
    queryFn: () => aplicacaoApi.list({ page, pageSize: 10, search }).then(r => r.data)
  })

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-white">Aplicacoes</h1>
          <p className="text-slate-400 mt-1">Gerenciar aplicacoes e sistemas</p>
        </div>
        <button className="flex items-center gap-2 bg-primary-600 hover:bg-primary-700 text-white px-4 py-2.5 rounded-lg transition-colors font-medium">
          <Plus className="w-4 h-4" />
          Nova Aplicacao
        </button>
      </div>

      <div className="flex items-center gap-4">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500" />
          <input
            type="text"
            placeholder="Buscar aplicacoes..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full bg-slate-900 border border-slate-800 rounded-lg pl-10 pr-4 py-2.5 text-sm text-white placeholder-slate-500 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {isLoading ? (
          [...Array(6)].map((_, i) => (
            <div key={i} className="bg-slate-900 border border-slate-800 rounded-xl p-6 animate-pulse">
              <div className="w-12 h-12 bg-slate-800 rounded-lg mb-4"></div>
              <div className="h-5 bg-slate-800 rounded mb-2"></div>
              <div className="h-4 bg-slate-800 rounded w-3/4"></div>
            </div>
          ))
        ) : data?.items.length === 0 ? (
          <div className="col-span-full text-center py-12 text-slate-500">Nenhuma aplicacao encontrada</div>
        ) : (
          data?.items.map((app) => (
            <div key={app.id} className="bg-slate-900 border border-slate-800 rounded-xl p-6 hover:border-slate-700 transition-colors group">
              <div className="flex items-start justify-between mb-4">
                <div className="w-12 h-12 rounded-lg bg-slate-800 flex items-center justify-center" style={{ backgroundColor: app.cor ? `${app.cor}20` : undefined }}>
                  <AppWindow className="w-6 h-6 text-primary-400" />
                </div>
                <div className="flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
                  <a href={app.url} target="_blank" rel="noopener noreferrer" className="p-1.5 rounded-md hover:bg-slate-800 text-slate-400 hover:text-white transition-colors">
                    <ExternalLink className="w-4 h-4" />
                  </a>
                  <button className="p-1.5 rounded-md hover:bg-slate-800 text-slate-400 hover:text-white transition-colors">
                    <Edit2 className="w-4 h-4" />
                  </button>
                  <button className="p-1.5 rounded-md hover:bg-red-500/10 text-slate-400 hover:text-red-400 transition-colors">
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              </div>
              <h3 className="text-lg font-semibold text-white">{app.nome}</h3>
              <p className="text-slate-400 text-sm mt-1">{app.descricao}</p>
              <div className="flex items-center gap-3 mt-4 pt-4 border-t border-slate-800">
                <span className="inline-flex items-center gap-1.5 px-2 py-0.5 rounded-md text-xs font-medium bg-slate-800 text-slate-300">
                  <Globe className="w-3 h-3" />
                  {app.codigo}
                </span>
                {app.isPublica && (
                  <span className="inline-flex px-2 py-0.5 rounded-md text-xs font-medium bg-blue-500/10 text-blue-400 border border-blue-500/20">
                    Publica
                  </span>
                )}
                <span className={`inline-flex px-2 py-0.5 rounded-md text-xs font-medium ${
                  app.ativa
                    ? 'bg-emerald-500/10 text-emerald-400 border border-emerald-500/20'
                    : 'bg-red-500/10 text-red-400 border border-red-500/20'
                }`}>
                  {app.ativa ? 'Ativa' : 'Inativa'}
                </span>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  )
}

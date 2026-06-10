import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Shield, Plus, Search, Edit2, Trash2, Key } from 'lucide-react'
import { perfilApi } from '@/services/api'
import type { Perfil, PaginatedResult } from '@/types/api'

export default function PerfisPage() {
  const [search, setSearch] = useState('')
  const [page] = useState(1)

  // Mock empresaId - in real app would come from context
  const empresaId = 'mock-empresa-id'

  const { data, isLoading } = useQuery<PaginatedResult<Perfil>>({
    queryKey: ['perfis', page, search],
    queryFn: () => perfilApi.list(empresaId, { page, pageSize: 10 }).then(r => r.data)
  })

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-white">Perfis</h1>
          <p className="text-slate-400 mt-1">Gerenciar perfis de acesso</p>
        </div>
        <button className="flex items-center gap-2 bg-primary-600 hover:bg-primary-700 text-white px-4 py-2.5 rounded-lg transition-colors font-medium">
          <Plus className="w-4 h-4" />
          Novo Perfil
        </button>
      </div>

      <div className="flex items-center gap-4">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500" />
          <input
            type="text"
            placeholder="Buscar perfis..."
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
          <div className="col-span-full text-center py-12 text-slate-500">Nenhum perfil encontrado</div>
        ) : (
          data?.items.map((perfil) => (
            <div key={perfil.id} className="bg-slate-900 border border-slate-800 rounded-xl p-6 hover:border-slate-700 transition-colors group">
              <div className="flex items-start justify-between mb-4">
                <div className="w-12 h-12 rounded-lg bg-primary-600/20 flex items-center justify-center">
                  <Shield className="w-6 h-6 text-primary-400" />
                </div>
                <div className="flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
                  <button className="p-1.5 rounded-md hover:bg-slate-800 text-slate-400 hover:text-white transition-colors">
                    <Edit2 className="w-4 h-4" />
                  </button>
                  <button className="p-1.5 rounded-md hover:bg-red-500/10 text-slate-400 hover:text-red-400 transition-colors">
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              </div>
              <h3 className="text-lg font-semibold text-white">{perfil.nome}</h3>
              <p className="text-slate-400 text-sm mt-1">{perfil.descricao}</p>
              <div className="flex items-center gap-4 mt-4 pt-4 border-t border-slate-800">
                <div className="flex items-center gap-1.5 text-sm text-slate-400">
                  <Key className="w-4 h-4" />
                  <span>{perfil.totalUsuarios} usuarios</span>
                </div>
                {perfil.isPadrao && (
                  <span className="inline-flex px-2 py-0.5 rounded-md text-xs font-medium bg-amber-500/10 text-amber-400 border border-amber-500/20">
                    Padrao
                  </span>
                )}
                <span className={`inline-flex px-2 py-0.5 rounded-md text-xs font-medium ${
                  perfil.ativo
                    ? 'bg-emerald-500/10 text-emerald-400 border border-emerald-500/20'
                    : 'bg-red-500/10 text-red-400 border border-red-500/20'
                }`}>
                  {perfil.ativo ? 'Ativo' : 'Inativo'}
                </span>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  )
}

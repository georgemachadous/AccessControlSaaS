import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Plus, Search, Edit2, Trash2, CheckCircle, XCircle, Loader2, User } from 'lucide-react'
import { usuarioApi } from '@/services/api'
import type { Usuario, PaginatedResult } from '@/types/api'

export default function UsuariosPage() {
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)

  const { data, isLoading } = useQuery<PaginatedResult<Usuario>>({
    queryKey: ['usuarios', page, search],
    queryFn: () => usuarioApi.list({ page, pageSize: 10, search }).then(r => r.data)
  })

  const handleToggleStatus = async (id: string, ativo: boolean) => {
    if (ativo) {
      await usuarioApi.deactivate(id)
    } else {
      await usuarioApi.activate(id)
    }
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-white">Usuarios</h1>
          <p className="text-slate-400 mt-1">Gerenciar usuarios do sistema</p>
        </div>
        <button className="flex items-center gap-2 bg-primary-600 hover:bg-primary-700 text-white px-4 py-2.5 rounded-lg transition-colors font-medium">
          <Plus className="w-4 h-4" />
          Novo Usuario
        </button>
      </div>

      <div className="flex items-center gap-4">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500" />
          <input
            type="text"
            placeholder="Buscar usuarios..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full bg-slate-900 border border-slate-800 rounded-lg pl-10 pr-4 py-2.5 text-sm text-white placeholder-slate-500 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="border-b border-slate-800">
              <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Usuario</th>
              <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Email</th>
              <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Idioma</th>
              <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Ultimo Acesso</th>
              <th className="text-left px-6 py-4 text-sm font-medium text-slate-400">Status</th>
              <th className="text-right px-6 py-4 text-sm font-medium text-slate-400">Acoes</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr><td colSpan={6} className="px-6 py-12 text-center"><Loader2 className="w-6 h-6 animate-spin mx-auto text-primary-500" /></td></tr>
            ) : data?.items.length === 0 ? (
              <tr><td colSpan={6} className="px-6 py-12 text-center text-slate-500">Nenhum usuario encontrado</td></tr>
            ) : (
              data?.items.map((usuario) => (
                <tr key={usuario.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                  <td className="px-6 py-4">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded-full bg-slate-800 flex items-center justify-center">
                        <User className="w-5 h-5 text-primary-400" />
                      </div>
                      <div>
                        <p className="text-white font-medium">{usuario.nome}</p>
                        <p className="text-slate-500 text-sm">{usuario.cpf || 'Sem CPF'}</p>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 text-slate-300">{usuario.email}</td>
                  <td className="px-6 py-4">
                    <span className="inline-flex px-2 py-1 rounded-md text-xs font-medium bg-slate-800 text-slate-300 uppercase">
                      {usuario.idioma}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-slate-300">
                    {usuario.ultimoAcesso
                      ? new Date(usuario.ultimoAcesso).toLocaleDateString('pt-BR')
                      : 'Nunca'}
                  </td>
                  <td className="px-6 py-4">
                    <button
                      onClick={() => handleToggleStatus(usuario.id, usuario.ativo)}
                      className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium transition-colors ${
                        usuario.ativo
                          ? 'bg-emerald-500/10 text-emerald-400 border border-emerald-500/20'
                          : 'bg-red-500/10 text-red-400 border border-red-500/20'
                      }`}
                    >
                      {usuario.ativo ? <CheckCircle className="w-3 h-3" /> : <XCircle className="w-3 h-3" />}
                      {usuario.ativo ? 'Ativo' : 'Inativo'}
                    </button>
                  </td>
                  <td className="px-6 py-4 text-right">
                    <div className="flex items-center justify-end gap-2">
                      <button className="p-2 rounded-lg hover:bg-slate-700 text-slate-400 hover:text-white transition-colors" title="Editar">
                        <Edit2 className="w-4 h-4" />
                      </button>
                      <button className="p-2 rounded-lg hover:bg-red-500/10 text-slate-400 hover:text-red-400 transition-colors" title="Excluir">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>

        {data && data.totalPages > 1 && (
          <div className="flex items-center justify-between px-6 py-4 border-t border-slate-800">
            <p className="text-sm text-slate-500">Mostrando {data.items.length} de {data.totalCount} resultados</p>
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

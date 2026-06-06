export interface Empresa {
  id: string
  nome: string
  nomeFantasia: string
  cnpj: string
  email: string
  telefone: string
  logoUrl: string
  idiomaPadrao: string
  ativa: boolean
  cep: string
  rua: string
  numero: string
  complemento: string
  bairro: string
  cidade: string
  estado: string
  pais: string
  createdAt: string
}

export interface Usuario {
  id: string
  nome: string
  email: string
  telefone?: string
  cpf?: string
  idioma: string
  avatarUrl?: string
  ativo: boolean
  emailConfirmado: boolean
  mfaHabilitado: boolean
  ultimoAcesso?: string
  filialId?: string
  createdAt: string
}

export interface Perfil {
  id: string
  empresaId: string
  nome: string
  descricao: string
  isPadrao: boolean
  ativo: boolean
  totalUsuarios: number
  createdAt: string
}

export interface Aplicacao {
  id: string
  nome: string
  descricao: string
  codigo: string
  url: string
  icone?: string
  cor?: string
  ativa: boolean
  isPublica: boolean
  createdAt: string
}

export interface Funcionalidade {
  id: string
  nome: string
  descricao: string
  codigo: string
  categoria: string
  icone?: string
  rota?: string
  ordem: number
  ativa: boolean
  isPadrao: boolean
  aplicacaoId?: string
}

export interface Notificacao {
  id: string
  titulo: string
  mensagem: string
  tipo: 'info' | 'success' | 'warning' | 'error'
  link?: string
  lida: boolean
  dataLeitura?: string
  createdAt: string
}

export interface LoginResponse {
  accessToken: string
  refreshToken: string
  expiresAt: string
  tokenType: string
  mfaRequired?: string
  usuario?: Usuario
}

export interface PaginatedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface DashboardStats {
  totalEmpresas: number
  totalUsuarios: number
  totalAplicacoes: number
  totalPerfis: number
  loginsHoje: number
  usuariosAtivos: number
  sessoesAtivas: number
  loginsPorDia: { data: string; quantidade: number }[]
}

export interface LogAuditoria {
  id: string
  acao: string
  entidade: string
  entidadeId: string
  valorAnterior?: string
  valorNovo?: string
  ipAddress: string
  userAgent: string
  endpoint?: string
  metodoHttp?: string
  statusCode?: number
  correlationId?: string
  observacao?: string
  createdAt: string
}

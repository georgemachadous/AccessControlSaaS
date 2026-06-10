import { useEffect, useState } from 'react';
import { crudApi } from '../api';
import { useToast } from '../components/ToastContainer';
import Modal from '../components/Modal';
import ConfirmDialog from '../components/ConfirmDialog';
import Breadcrumb from '../components/Breadcrumb';
import EmptyState from '../components/EmptyState';
import Badge from '../components/Badge';
import Skeleton from '../components/Skeleton';
import {
  PlusIcon, EditIcon, TrashIcon, SearchIcon, FilterIcon,
  DownloadIcon, XIcon
} from '../components/Icons';
import type { EntityType } from '../types';

interface CrudPageProps { type: EntityType; title: string; }

interface FieldConfig {
  key: string;
  label: string;
  type: 'text' | 'email' | 'password' | 'select' | 'checkbox' | 'textarea';
  options?: string[];
  required?: boolean;
  grid?: 'half' | 'full';
}

const configs: Record<EntityType, { fields: FieldConfig[] }> = {
  companies: { fields: [
    { key: 'name', label: 'Nome', type: 'text', required: true },
    { key: 'document', label: 'Documento (CNPJ)', type: 'text', grid: 'half' },
    { key: 'phone', label: 'Telefone', type: 'text', grid: 'half' },
    { key: 'email', label: 'Email', type: 'email' },
    { key: 'isActive', label: 'Ativo', type: 'checkbox', grid: 'half' },
  ]},
  branches: { fields: [
    { key: 'name', label: 'Nome', type: 'text', required: true },
    { key: 'address', label: 'Endereco', type: 'textarea' },
    { key: 'phone', label: 'Telefone', type: 'text', grid: 'half' },
    { key: 'companyId', label: 'Empresa ID', type: 'text', required: true, grid: 'half' },
    { key: 'isActive', label: 'Ativo', type: 'checkbox', grid: 'half' },
  ]},
  applications: { fields: [
    { key: 'name', label: 'Nome', type: 'text', required: true },
    { key: 'description', label: 'Descricao', type: 'textarea' },
    { key: 'url', label: 'URL', type: 'text', grid: 'half' },
    { key: 'icon', label: 'Icone', type: 'text', grid: 'half' },
    { key: 'companyId', label: 'Empresa ID', type: 'text', required: true, grid: 'half' },
    { key: 'isActive', label: 'Ativo', type: 'checkbox', grid: 'half' },
  ]},
  functionalities: { fields: [
    { key: 'name', label: 'Nome', type: 'text', required: true },
    { key: 'description', label: 'Descricao', type: 'textarea' },
    { key: 'code', label: 'Codigo', type: 'text', grid: 'half' },
    { key: 'applicationId', label: 'Aplicacao ID', type: 'text', required: true, grid: 'half' },
    { key: 'isActive', label: 'Ativo', type: 'checkbox', grid: 'half' },
  ]},
  roles: { fields: [
    { key: 'name', label: 'Nome', type: 'text', required: true },
    { key: 'description', label: 'Descricao', type: 'textarea' },
    { key: 'applicationId', label: 'Aplicacao ID', type: 'text', required: true, grid: 'half' },
    { key: 'isActive', label: 'Ativo', type: 'checkbox', grid: 'half' },
  ]},
  rolefunctionalities: { fields: [
    { key: 'canCreate', label: 'Criar', type: 'checkbox', grid: 'half' },
    { key: 'canRead', label: 'Ler', type: 'checkbox', grid: 'half' },
    { key: 'canUpdate', label: 'Atualizar', type: 'checkbox', grid: 'half' },
    { key: 'canDelete', label: 'Deletar', type: 'checkbox', grid: 'half' },
    { key: 'roleId', label: 'Perfil ID', type: 'text', required: true, grid: 'half' },
    { key: 'functionalityId', label: 'Funcionalidade ID', type: 'text', required: true, grid: 'half' },
  ]},
  users: { fields: [
    { key: 'name', label: 'Nome Completo', type: 'text', required: true },
    { key: 'email', label: 'Email', type: 'email', required: true },
    { key: 'password', label: 'Senha', type: 'password', required: true, grid: 'half' },
    { key: 'companyId', label: 'Empresa ID', type: 'text', grid: 'half' },
    { key: 'isSuperAdmin', label: 'Super Admin', type: 'checkbox', grid: 'half' },
    { key: 'ssoProvider', label: 'Provedor SSO', type: 'text', grid: 'half' },
  ]},
  userapplicationroles: { fields: [
    { key: 'userId', label: 'Usuario ID', type: 'text', required: true, grid: 'half' },
    { key: 'applicationId', label: 'Aplicacao ID', type: 'text', required: true, grid: 'half' },
    { key: 'roleId', label: 'Perfil ID', type: 'text', required: true, grid: 'half' },
  ]},
  permissions: { fields: [
    { key: 'resource', label: 'Recurso', type: 'text', required: true },
    { key: 'action', label: 'Acao', type: 'text', required: true, grid: 'half' },
    { key: 'isAllowed', label: 'Permitido', type: 'checkbox', grid: 'half' },
    { key: 'userId', label: 'Usuario ID', type: 'text', required: true, grid: 'half' },
  ]},
};

export default function CrudPage({ type, title }: CrudPageProps) {
  const [items, setItems] = useState<Record<string, unknown>[]>([]);
  const [filteredItems, setFilteredItems] = useState<Record<string, unknown>[]>([]);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [formData, setFormData] = useState<Record<string, unknown>>({});
  const [searchQuery, setSearchQuery] = useState('');
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const api = crudApi(type);
  const config = configs[type];
  const { showToast } = useToast();

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await api.getAll();
      setItems(data);
      setFilteredItems(data);
    } catch {
      showToast('Erro ao carregar dados', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, [type]);

  useEffect(() => {
    if (!searchQuery) { setFilteredItems(items); return; }
    const q = searchQuery.toLowerCase();
    setFilteredItems(items.filter(item =>
      Object.values(item).some(v => String(v).toLowerCase().includes(q))
    ));
  }, [searchQuery, items]);

  const openCreate = () => {
    setEditingId(null);
    setFormData({});
    setModalOpen(true);
  };

  const openEdit = (item: Record<string, unknown>) => {
    setEditingId(item.id as string);
    const cleanData = { ...item };
    config.fields.forEach(f => {
      if (f.type === 'checkbox') cleanData[f.key] = !!cleanData[f.key];
    });
    setFormData(cleanData);
    setModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const data = { ...formData };
      config.fields.forEach(f => {
        if (f.type === 'checkbox') data[f.key] = !!data[f.key];
      });
      if (editingId) {
        await api.update(editingId, data);
        showToast(`${title} atualizado com sucesso`, 'success');
      } else {
        await api.create(data);
        showToast(`${title} criado com sucesso`, 'success');
      }
      setModalOpen(false);
      load();
    } catch {
      showToast('Erro ao salvar', 'error');
    }
  };

  const handleDelete = async () => {
    if (!deleteId) return;
    try {
      await api.delete(deleteId);
      showToast(`${title} excluido com sucesso`, 'success');
      load();
    } catch {
      showToast('Erro ao excluir', 'error');
    }
    setDeleteId(null);
  };

  const confirmDelete = (id: string) => {
    setDeleteId(id);
    setConfirmOpen(true);
  };

  const handleChange = (key: string, value: unknown) => {
    setFormData(prev => ({ ...prev, [key]: value }));
  };

  const renderField = (field: FieldConfig) => {
    const value = formData[field.key] ?? '';
    const labelStyle = {
      display: 'block',
      fontSize: 'var(--font-xs)',
      fontWeight: 600,
      textTransform: 'uppercase' as const,
      letterSpacing: '0.5px',
      color: 'var(--text-muted)',
      marginBottom: 'var(--space-sm)',
    };

    if (field.type === 'checkbox') {
      return (
        <div key={field.key} style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-md)', padding: 'var(--space-sm) 0' }}>
          <input
            type="checkbox"
            className="checkbox"
            checked={!!value}
            onChange={e => handleChange(field.key, e.target.checked)}
            id={field.key}
          />
          <label htmlFor={field.key} style={{ fontSize: 'var(--font-sm)', color: 'var(--text-secondary)', cursor: 'pointer', fontWeight: 500 }}>
            {field.label}
          </label>
        </div>
      );
    }

    if (field.type === 'textarea') {
      return (
        <div key={field.key} style={{ marginBottom: 'var(--space-lg)' }}>
          <label style={labelStyle}>{field.label}{field.required && <span style={{ color: 'var(--danger)' }}> *</span>}</label>
          <textarea
            className="input"
            rows={3}
            value={String(value)}
            onChange={e => handleChange(field.key, e.target.value)}
            placeholder={`Digite ${field.label.toLowerCase()}`}
            style={{ resize: 'vertical' }}
          />
        </div>
      );
    }

    return (
      <div key={field.key} style={{ marginBottom: 'var(--space-lg)', gridColumn: field.grid === 'half' ? 'span 1' : 'span 2' }}>
        <label style={labelStyle}>{field.label}{field.required && <span style={{ color: 'var(--danger)' }}> *</span>}</label>
        <input
          type={field.type}
          className="input"
          value={String(value)}
          onChange={e => handleChange(field.key, e.target.value)}
          placeholder={`Digite ${field.label.toLowerCase()}`}
        />
      </div>
    );
  };

  const tableFields = config.fields.filter(f => f.type !== 'password');

  return (
    <div>
      <Breadcrumb items={[{ label: 'Dashboard', path: '/' }, { label: title }]} />

      <div className="page-header">
        <div>
          <h1 className="page-title">{title}</h1>
          <p className="page-subtitle">Gerencie {title.toLowerCase()} do sistema</p>
        </div>
        <button onClick={openCreate} className="btn btn-primary">
          <PlusIcon size={16} /> Novo {title.slice(0, -1)}
        </button>
      </div>

      <div className="toolbar">
        <div className="toolbar-search">
          <span style={{ position: 'absolute', left: 12, top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }}>
            <SearchIcon size={16} />
          </span>
          <input
            type="text"
            placeholder={`Buscar ${title.toLowerCase()}...`}
            value={searchQuery}
            onChange={e => setSearchQuery(e.target.value)}
          />
          {searchQuery && (
            <button
              onClick={() => setSearchQuery('')}
              style={{ position: 'absolute', right: 12, top: '50%', transform: 'translateY(-50%)', background: 'none', border: 'none', color: 'var(--text-muted)', cursor: 'pointer' }}
            >
              <XIcon size={14} />
            </button>
          )}
        </div>
        <div style={{ display: 'flex', gap: 'var(--space-sm)' }}>
          <button className="btn btn-secondary btn-sm"><FilterIcon size={14} /> Filtros</button>
          <button className="btn btn-secondary btn-sm"><DownloadIcon size={14} /> Exportar</button>
        </div>
      </div>

      <div className="table-container">
        {loading ? (
          <div style={{ padding: 'var(--space-xl)' }}>
            {[1,2,3,4,5].map(i => (
              <div key={i} style={{ display: 'flex', gap: 'var(--space-lg)', padding: 'var(--space-md) 0', borderBottom: '1px solid var(--border-default)' }}>
                <Skeleton height={16} width="25%" />
                <Skeleton height={16} width="20%" />
                <Skeleton height={16} width="15%" />
                <Skeleton height={16} width="15%" />
                <Skeleton height={16} width="10%" />
              </div>
            ))}
          </div>
        ) : filteredItems.length === 0 ? (
          <EmptyState
            title={searchQuery ? 'Nenhum resultado' : `Nenhum ${title.slice(0, -1).toLowerCase()} cadastrado`}
            description={searchQuery ? 'Tente ajustar sua busca.' : `Clique em "Novo" para adicionar o primeiro ${title.slice(0, -1).toLowerCase()}.`}
            action={!searchQuery && <button onClick={openCreate} className="btn btn-primary"><PlusIcon size={16} /> Novo {title.slice(0, -1)}</button>}
          />
        ) : (
          <table className="table">
            <thead>
              <tr>
                {tableFields.map(f => <th key={f.key}>{f.label}</th>)}
                <th style={{ textAlign: 'right' }}>Acoes</th>
              </tr>
            </thead>
            <tbody>
              {filteredItems.map(item => (
                <tr key={item.id as string}>
                  {tableFields.map(f => (
                    <td key={f.key}>
                      {f.type === 'checkbox'
                        ? (item[f.key] ? <Badge variant="success">Sim</Badge> : <Badge variant="danger">Nao</Badge>)
                        : f.key === 'name'
                          ? <span style={{ color: 'var(--text-primary)', fontWeight: 500 }}>{String(item[f.key] ?? '')}</span>
                          : String(item[f.key] ?? '')
                      }
                    </td>
                  ))}
                  <td style={{ textAlign: 'right' }}>
                    <div style={{ display: 'flex', gap: 'var(--space-sm)', justifyContent: 'flex-end' }}>
                      <button onClick={() => openEdit(item)} className="btn btn-ghost btn-sm" title="Editar">
                        <EditIcon size={14} />
                      </button>
                      <button onClick={() => confirmDelete(item.id as string)} className="btn btn-danger btn-sm" title="Excluir">
                        <TrashIcon size={14} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      <Modal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        title={`${editingId ? 'Editar' : 'Novo'} ${title.slice(0, -1)}`}
        footer={
          <>
            <button onClick={() => setModalOpen(false)} className="btn btn-secondary">Cancelar</button>
            <button onClick={handleSubmit} className="btn btn-primary">{editingId ? 'Salvar Alteracoes' : 'Criar'}</button>
          </>
        }
      >
        <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0 var(--space-lg)' }}>
          {config.fields.map(renderField)}
        </form>
      </Modal>

      <ConfirmDialog
        isOpen={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={handleDelete}
        title="Confirmar exclusao"
        message={`Tem certeza que deseja excluir este ${title.slice(0, -1).toLowerCase()}? Esta acao nao pode ser desfeita.`}
        confirmText="Excluir"
      />
    </div>
  );
}

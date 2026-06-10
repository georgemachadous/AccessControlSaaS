import { useEffect, useState } from 'react';
import { dashboardApi } from '../api';
import StatCard from '../components/StatCard';
import Breadcrumb from '../components/Breadcrumb';
import EmptyState from '../components/EmptyState';
import Skeleton from '../components/Skeleton';
import {
  BuildingIcon, AppIcon, UsersIcon, RoleIcon,
  ActivityIcon, TrendingUpIcon
} from '../components/Icons';
import type { DashboardSummary, RecentActivity } from '../types';

export default function Dashboard() {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);
  const [activities, setActivities] = useState<RecentActivity[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      dashboardApi.summary(),
      dashboardApi.activity(5)
    ]).then(([s, a]) => {
      setSummary(s.data);
      setActivities(a.data);
      setLoading(false);
    });
  }, []);

  return (
    <div>
      <Breadcrumb items={[{ label: 'Dashboard' }]} />

      <div className="page-header">
        <div>
          <h1 className="page-title">Dashboard</h1>
          <p className="page-subtitle">Visao geral do sistema</p>
        </div>
      </div>

      {loading ? (
        <div className="stats-grid">
          {[1,2,3,4].map(i => (
            <div className="stat-card" key={i}>
              <Skeleton height={40} width={80} />
              <Skeleton height={14} width={120} />
            </div>
          ))}
        </div>
      ) : (
        <div className="stats-grid">
          <StatCard title="Empresas" value={summary?.totalCompanies || 0} icon={<BuildingIcon size={20} />} iconColor="primary" />
          <StatCard title="Aplicacoes" value={summary?.totalApplications || 0} icon={<AppIcon size={20} />} iconColor="primary" />
          <StatCard title="Usuarios" value={summary?.totalUsers || 0} icon={<UsersIcon size={20} />} iconColor="success" change={12} changeLabel="este mes" />
          <StatCard title="Perfis" value={summary?.totalRoles || 0} icon={<RoleIcon size={20} />} iconColor="warning" />
        </div>
      )}

      <div className="stats-grid" style={{ marginTop: 'var(--space-lg)' }}>
        <StatCard title="Usuarios Ativos" value={summary?.activeUsers || 0} icon={<TrendingUpIcon size={20} />} iconColor="success" />
        <StatCard title="Usuarios Inativos" value={summary?.inactiveUsers || 0} icon={<ActivityIcon size={20} />} iconColor="danger" />
      </div>

      <div className="card" style={{ marginTop: 'var(--space-xl)' }}>
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 'var(--space-lg)' }}>
          <h3 style={{ fontSize: 'var(--font-lg)', fontWeight: 600 }}>Atividade Recente</h3>
        </div>
        {activities.length === 0 ? (
          <EmptyState title="Nenhuma atividade" description="As acoes realizadas no sistema aparecerao aqui." />
        ) : (
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Descricao</th>
                  <th>Tipo</th>
                  <th style={{ textAlign: 'right' }}>Data</th>
                </tr>
              </thead>
              <tbody>
                {activities.map((a, i) => (
                  <tr key={i}>
                    <td style={{ color: 'var(--text-primary)', fontWeight: 500 }}>{a.description}</td>
                    <td><span className="badge badge-default">{a.type}</span></td>
                    <td style={{ textAlign: 'right', fontSize: 'var(--font-xs)', color: 'var(--text-muted)' }}>
                      {new Date(a.date).toLocaleString('pt-BR')}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}

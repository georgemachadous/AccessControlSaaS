import React from 'react';
import { SearchIcon } from './Icons';

interface EmptyStateProps {
  title: string;
  description: string;
  action?: React.ReactNode;
}

export default function EmptyState({ title, description, action }: EmptyStateProps) {
  return (
    <div className="empty-state">
      <div className="empty-state-icon"><SearchIcon size={28} /></div>
      <h3>{title}</h3>
      <p>{description}</p>
      {action && <div style={{ marginTop: 'var(--space-lg)' }}>{action}</div>}
    </div>
  );
}

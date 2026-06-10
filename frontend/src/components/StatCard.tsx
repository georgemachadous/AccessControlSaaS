import React from 'react';
import { TrendingUpIcon, TrendingDownIcon } from './Icons';

interface StatCardProps {
  title: string;
  value: string | number;
  icon: React.ReactNode;
  iconColor?: 'primary' | 'success' | 'danger' | 'warning';
  change?: number;
  changeLabel?: string;
}

export default function StatCard({ title, value, icon, iconColor = 'primary', change, changeLabel }: StatCardProps) {
  const isPositive = change && change >= 0;

  return (
    <div className="stat-card">
      <div className="stat-header">
        <div>
          <div className="stat-value">{value}</div>
          <div className="stat-label">{title}</div>
        </div>
        <div className={`stat-icon ${iconColor}`}>{icon}</div>
      </div>
      {change !== undefined && (
        <div className={`stat-change ${isPositive ? 'positive' : 'negative'}`}>
          {isPositive ? <TrendingUpIcon size={12} /> : <TrendingDownIcon size={12} />}
          {Math.abs(change)}% {changeLabel}
        </div>
      )}
    </div>
  );
}

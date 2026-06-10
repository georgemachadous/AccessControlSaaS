import React from 'react';
import { SearchIcon, BellIcon } from './Icons';

interface HeaderProps {
  title: string;
  subtitle?: string;
}

export default function Header({ title, subtitle }: HeaderProps) {
  return (
    <header className="header">
      <div className="header-search">
        <span className="header-search-icon"><SearchIcon size={16} /></span>
        <input type="text" placeholder="Buscar..." />
      </div>
      <div className="header-actions">
        <button className="header-btn">
          <BellIcon size={18} />
          <span className="header-badge" />
        </button>
      </div>
    </header>
  );
}

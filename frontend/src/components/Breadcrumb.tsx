import React from 'react';
import { Link } from 'react-router-dom';
import { ChevronRightIcon } from './Icons';

interface BreadcrumbItem {
  label: string;
  path?: string;
}

interface BreadcrumbProps {
  items: BreadcrumbItem[];
}

export default function Breadcrumb({ items }: BreadcrumbProps) {
  return (
    <nav className="breadcrumb">
      {items.map((item, index) => (
        <React.Fragment key={index}>
          {index > 0 && <span className="breadcrumb-sep"><ChevronRightIcon size={14} /></span>}
          {item.path ? (
            <Link to={item.path}>{item.label}</Link>
          ) : (
            <span style={{ color: 'var(--text-primary)' }}>{item.label}</span>
          )}
        </React.Fragment>
      ))}
    </nav>
  );
}

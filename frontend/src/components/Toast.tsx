import React, { useEffect } from 'react';
import { CheckIcon, AlertTriangleIcon, XIcon } from './Icons';

interface ToastProps {
  id: string;
  message: string;
  type?: 'success' | 'error' | 'warning';
  onClose: (id: string) => void;
  duration?: number;
}

export default function Toast({ id, message, type = 'success', onClose, duration = 4000 }: ToastProps) {
  useEffect(() => {
    const timer = setTimeout(() => onClose(id), duration);
    return () => clearTimeout(timer);
  }, [id, duration, onClose]);

  const icon = type === 'success' ? <CheckIcon size={18} /> : <AlertTriangleIcon size={18} />;
  const color = type === 'success' ? 'var(--success)' : type === 'error' ? 'var(--danger)' : 'var(--warning)';

  return (
    <div className={`toast toast-${type}`}>
      <span style={{ color, flexShrink: 0 }}>{icon}</span>
      <span style={{ flex: 1, fontSize: 'var(--font-sm)', color: 'var(--text-primary)' }}>{message}</span>
      <button onClick={() => onClose(id)} className="header-btn" style={{ width: 24, height: 24 }}>
        <XIcon size={14} />
      </button>
    </div>
  );
}

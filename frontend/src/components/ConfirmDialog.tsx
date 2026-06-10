import React from 'react';
import Modal from './Modal';
import { AlertTriangleIcon } from './Icons';

interface ConfirmDialogProps {
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => void;
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
}

export default function ConfirmDialog({ isOpen, onClose, onConfirm, title, message, confirmText = 'Excluir', cancelText = 'Cancelar' }: ConfirmDialogProps) {
  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={title}
      size="sm"
      footer={
        <>
          <button onClick={onClose} className="btn btn-secondary">{cancelText}</button>
          <button onClick={() => { onConfirm(); onClose(); }} className="btn btn-danger">{confirmText}</button>
        </>
      }
    >
      <div className="confirm-dialog">
        <div className="confirm-dialog-icon"><AlertTriangleIcon size={28} /></div>
        <p style={{ color: 'var(--text-secondary)', fontSize: 'var(--font-sm)', lineHeight: 1.6 }}>{message}</p>
      </div>
    </Modal>
  );
}

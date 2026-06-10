import React from 'react';

interface SkeletonProps {
  width?: string | number;
  height?: string | number;
  circle?: boolean;
}

export default function Skeleton({ width = '100%', height = 16, circle = false }: SkeletonProps) {
  return (
    <div
      className="skeleton"
      style={{
        width: typeof width === 'number' ? `${width}px` : width,
        height: typeof height === 'number' ? `${height}px` : height,
        borderRadius: circle ? '50%' : 'var(--radius-md)',
      }}
    />
  );
}

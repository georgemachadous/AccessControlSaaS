interface SkeletonProps {
  className?: string
  width?: string | number
  height?: string | number
  circle?: boolean
}

export function Skeleton({ className = '', width, height, circle }: SkeletonProps) {
  const style: React.CSSProperties = {}
  if (width) style.width = typeof width === 'number' ? `${width}px` : width
  if (height) style.height = typeof height === 'number' ? `${height}px` : height

  return (
    <div
      className={`animate-pulse bg-slate-800 ${circle ? 'rounded-full' : 'rounded-md'} ${className}`}
      style={style}
    />
  )
}

export function SkeletonCard() {
  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl p-6 space-y-4">
      <div className="flex items-center gap-4">
        <Skeleton width={48} height={48} circle />
        <div className="space-y-2 flex-1">
          <Skeleton width="60%" height={20} />
          <Skeleton width="40%" height={16} />
        </div>
      </div>
      <Skeleton width="100%" height={16} />
      <Skeleton width="80%" height={16} />
    </div>
  )
}

export function SkeletonTable({ rows = 5 }: { rows?: number }) {
  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl overflow-hidden">
      <div className="h-12 bg-slate-800/50 border-b border-slate-800 flex items-center px-6 gap-6">
        {[...Array(6)].map((_, i) => (
          <Skeleton key={i} width={`${15 + Math.random() * 10}%`} height={16} />
        ))}
      </div>
      {[...Array(rows)].map((_, i) => (
        <div key={i} className="h-14 border-b border-slate-800 flex items-center px-6 gap-6">
          {[...Array(6)].map((_, j) => (
            <Skeleton key={j} width={`${15 + Math.random() * 10}%`} height={16} />
          ))}
        </div>
      ))}
    </div>
  )
}

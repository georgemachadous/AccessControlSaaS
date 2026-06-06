import { createContext, useContext, useEffect, type ReactNode } from 'react'
import { useThemeStore } from '@/stores/themeStore'

const ThemeContext = createContext<null>(null)

export function ThemeProvider({ children }: { children: ReactNode }) {
  const { theme } = useThemeStore()

  useEffect(() => {
    const isDark = theme === 'dark' || (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches)
    document.documentElement.classList.toggle('dark', isDark)
  }, [theme])

  return <ThemeContext.Provider value={null}>{children}</ThemeContext.Provider>
}

export function useTheme() {
  return useThemeStore()
}

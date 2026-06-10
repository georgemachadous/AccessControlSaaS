import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface ThemeState {
  theme: 'dark' | 'light' | 'system'
  isDark: boolean
  setTheme: (theme: 'dark' | 'light' | 'system') => void
  toggleTheme: () => void
}

export const useThemeStore = create<ThemeState>()(
  persist(
    (set, get) => ({
      theme: 'dark',
      isDark: true,
      setTheme: (theme) => {
        const isDark = theme === 'dark' || (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches)
        set({ theme, isDark })
        document.documentElement.classList.toggle('dark', isDark)
      },
      toggleTheme: () => {
        const newTheme = get().isDark ? 'light' : 'dark'
        get().setTheme(newTheme)
      }
    }),
    { name: 'theme-storage' }
  )
)

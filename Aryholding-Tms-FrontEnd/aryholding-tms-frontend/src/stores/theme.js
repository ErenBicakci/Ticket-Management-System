import { defineStore } from 'pinia'

export const useThemeStore = defineStore('theme', {
  state: () => ({
    isDark: false
  }),
  getters: {
    currentTheme: (state) => (state.isDark ? 'dark' : 'light')
  },
  actions: {
    initTheme() {
      const saved = localStorage.getItem('tms_theme')
      if (saved) {
        this.isDark = saved === 'dark'
      } else {
        this.isDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
      }
      this.applyTheme()
    },
    toggleTheme() {
      this.isDark = !this.isDark
      localStorage.setItem('tms_theme', this.isDark ? 'dark' : 'light')
      this.applyTheme()
    },
    applyTheme() {
      if (typeof document !== 'undefined') {
        document.documentElement.setAttribute('data-theme', this.isDark ? 'dark' : 'light')
        if (this.isDark) {
          document.documentElement.classList.add('dark')
        } else {
          document.documentElement.classList.remove('dark')
        }
      }
    }
  }
})

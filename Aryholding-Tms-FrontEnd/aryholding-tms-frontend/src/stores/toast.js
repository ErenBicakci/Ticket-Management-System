import { defineStore } from 'pinia'

let nextToastId = 1

export const useToastStore = defineStore('toast', {
  state: () => ({
    toasts: []
  }),

  actions: {
    addToast({ type = 'info', title = '', message = '', duration = 4000 }) {
      const id = nextToastId++
      const toast = {
        id,
        type, // 'success' | 'error' | 'warning' | 'info'
        title,
        message,
        duration
      }

      this.toasts.push(toast)

      if (duration > 0) {
        setTimeout(() => {
          this.removeToast(id)
        }, duration)
      }

      return id
    },

    success(message, title = 'Başarılı', duration = 3500) {
      return this.addToast({ type: 'success', title, message, duration })
    },

    error(message, title = 'Hata', duration = 5000) {
      return this.addToast({ type: 'error', title, message, duration })
    },

    warning(message, title = 'Uyarı', duration = 4000) {
      return this.addToast({ type: 'warning', title, message, duration })
    },

    info(message, title = 'Bilgi', duration = 3500) {
      return this.addToast({ type: 'info', title, message, duration })
    },

    removeToast(id) {
      const index = this.toasts.findIndex(t => t.id === id)
      if (index !== -1) {
        this.toasts.splice(index, 1)
      }
    },

    clearAll() {
      this.toasts = []
    }
  }
})

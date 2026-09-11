import axios from 'axios'
import { useAuthStore } from '../stores/auth.js'
import { useToastStore } from '../stores/toast.js'
import router from '../router/index.js'

const apiClient = axios.create({
  baseURL: '',
  headers: {
    'Content-Type': 'application/json'
  },
  timeout: 30000
})

apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

apiClient.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      const authStore = useAuthStore()
      authStore.logout()

      const toastStore = useToastStore()
      toastStore.warning('Oturum süreniz doldu. Lütfen tekrar giriş yapın.', 'Oturum Kapatıldı')

      if (router && router.currentRoute.value.path !== '/login') {
        router.push('/login')
      }
    }
    return Promise.reject(error)
  }
)

export default apiClient

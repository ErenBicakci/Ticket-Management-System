import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => {
    let storedUser = null
    try {
      const raw = localStorage.getItem('userInfo')
      if (raw) storedUser = JSON.parse(raw)
    } catch (e) {
      storedUser = null
    }

    return {
      token: localStorage.getItem('authToken') || null,
      userInfo: storedUser
    }
  },

  getters: {
    isAuthenticated: (state) => Boolean(state.token),
    userName: (state) => state.userInfo?.userName || 'Kullanıcı',
    userEmail: (state) => state.userInfo?.email || '',
    userInitials: (state) => {
      const name = state.userInfo?.userName || 'K'
      return name.charAt(0).toUpperCase()
    },
    roles: (state) => state.userInfo?.roles || [],
    departmentCodes: (state) => state.userInfo?.departmentCode || [],
    primaryDepartment: (state) => {
      const depts = state.userInfo?.departmentCode
      if (Array.isArray(depts) && depts.length > 0) return depts[0]
      return 'Kullanıcı'
    },
    priorities: (state) => state.userInfo?.priority || [],
    isSuperUser: (state) => state.userInfo?.superUser === true,
    canViewStats: (state) => {
      if (!state.userInfo) return false
      if (state.userInfo.superUser === true) return true
      const priorities = Array.isArray(state.userInfo.priority) ? state.userInfo.priority : []
      return priorities.some(p => parseInt(p, 10) >= 7)
    },
    canCreateDepartment: (state) => {
      if (!state.userInfo) return false
      if (state.userInfo.superUser === true) return true
      const deptCodes = Array.isArray(state.userInfo.departmentCode) ? state.userInfo.departmentCode : []
      const priorities = Array.isArray(state.userInfo.priority) ? state.userInfo.priority : []
      const itIndex = deptCodes.findIndex(code => String(code).toUpperCase() === 'IT')
      const itPriority = itIndex >= 0 ? parseInt(priorities[itIndex] || '0', 10) : 0
      return itIndex >= 0 && Number.isFinite(itPriority) && itPriority >= 7
    }
  },

  actions: {
    decodeJWT(token) {
      try {
        const base64Url = token.split('.')[1]
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(
          atob(base64)
            .split('')
            .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
            .join('')
        )
        const payload = JSON.parse(jsonPayload)

        return {
          userName: payload.username,
          email: payload.email,
          roles: payload.role || [],
          superUser: payload.superUser === 'true' || payload.superUser === true,
          departmentCode: Array.isArray(payload.departmentCode)
            ? payload.departmentCode
            : payload.departmentCode
            ? [payload.departmentCode]
            : [],
          priority: Array.isArray(payload.priority)
            ? payload.priority
            : payload.priority
            ? [payload.priority]
            : [],
          exp: payload.exp,
          iat: payload.iat
        }
      } catch (error) {
        console.error('JWT decode hatası:', error)
        return null
      }
    },

    setAuth(token, userInfo = null) {
      this.token = token
      localStorage.setItem('authToken', token)

      const resolvedUser = userInfo || this.decodeJWT(token)
      if (resolvedUser) {
        this.userInfo = resolvedUser
        localStorage.setItem('userInfo', JSON.stringify(resolvedUser))
      }
    },

    logout() {
      this.token = null
      this.userInfo = null
      localStorage.removeItem('authToken')
      localStorage.removeItem('userInfo')
    },

    checkAuth() {
      const token = localStorage.getItem('authToken')
      if (!token) {
        this.logout()
        return false
      }

      const decoded = this.decodeJWT(token)
      if (!decoded) {
        this.logout()
        return false
      }

      // Token süresi kontrolü (varsa exp)
      if (decoded.exp && decoded.exp * 1000 < Date.now()) {
        this.logout()
        return false
      }

      this.token = token
      this.userInfo = decoded
      return true
    }
  }
})

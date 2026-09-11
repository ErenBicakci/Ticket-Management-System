<template>
  <div class="departments-page">
    <div class="departments-header">
      <div class="header-icon">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
          <rect x="10" y="13" width="4" height="8"/>
        </svg>
      </div>
      <div class="header-text">
        <h2>Departman Seçimi</h2>
        <p>Çalışmak istediğiniz departmanı seçiniz</p>
      </div>
    </div>
    
    <div class="departments-grid">
      <div
        v-if="canCreateDepartment"
        class="department-card create-card"
        @click="showCreateModal = true"
      >
        <div class="dept-icon create-icon">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <line x1="12" y1="8" x2="12" y2="16"/>
            <line x1="8" y1="12" x2="16" y2="12"/>
          </svg>
        </div>
        <h3>Departman Oluştur</h3>
        <p>Yeni departman ekle</p>
        <div class="card-arrow">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="9 18 15 12 9 6"/>
          </svg>
        </div>
      </div>

      <div v-for="dept in departments" :key="dept.code" 
           class="department-card"
           @click="selectDepartment(dept)">
        <div class="dept-icon">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
            <rect x="10" y="13" width="4" height="8"/>
          </svg>
        </div>
        <h3>{{ dept.name || dept.code }}</h3>
        <p>{{ dept.code }}</p>
        <div class="card-arrow">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="9 18 15 12 9 6"/>
          </svg>
        </div>
      </div>
    </div>

    <DepartmentCreateModal
      :visible="showCreateModal"
      @close="showCreateModal = false"
      @created="onDepartmentCreated"
    />
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import DepartmentCreateModal from './DepartmentCreateModal.vue'
export default {
  name: 'DepartmentsPage',
  components: { DepartmentCreateModal },
  data() {
    return {
      departments: [],
      isLoading: false,
      canCreateDepartment: false,
      showCreateModal: false
    }
  },
  mounted() {
    this.fetchDepartments()
    this.evaluateCreatePermission()
  },
  methods: {
    evaluateCreatePermission() {
      try {
        const stored = localStorage.getItem('userInfo')
        let userInfo = null
        if (stored) {
          userInfo = JSON.parse(stored)
        } else {
          const token = localStorage.getItem('authToken')
          if (token) userInfo = this.decodeJWT(token)
        }

        if (!userInfo) { this.canCreateDepartment = false; return }
        if (userInfo.superUser === true) { this.canCreateDepartment = true; return }

        const deptCodes = Array.isArray(userInfo.departmentCode) ? userInfo.departmentCode : []
        const priorities = Array.isArray(userInfo.priority) ? userInfo.priority : []
        const itIndex = deptCodes.findIndex(code => String(code).toUpperCase() === 'IT')
        const itPriority = itIndex >= 0 ? parseInt(priorities[itIndex] || '0', 10) : 0
        this.canCreateDepartment = itIndex >= 0 && Number.isFinite(itPriority) && itPriority >= 7
      } catch (e) {
        this.canCreateDepartment = false
      }
    },

    decodeJWT(token) {
      try {
        const base64Url = token.split('.')[1]
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        }).join(''))
        const payload = JSON.parse(jsonPayload)
        return {
          userName: payload.username,
          email: payload.email,
          roles: payload.role || [],
          superUser: payload.superUser === 'true' || payload.superUser === true,
          departmentCode: payload.departmentCode || [],
          priority: payload.priority || []
        }
      } catch (error) {
        return null
      }
    },

    async fetchDepartments() {
      this.isLoading = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/department/get-all', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) throw await errorManager.createErrorFromResponse(response)

        const data = await response.json()
        this.departments = Array.isArray(data) ? data : []
      } catch (error) {
        errorManager.logError('Fetch departments', error)
        errorManager.showError('Departmanlar yüklenemedi', error)
      } finally {
        this.isLoading = false
      }
    },

    selectDepartment(department) {
      this.$emit('select-department', department.code)
      if (this.$router) {
        this.$router.push(`/departments/${department.code}`)
      }
    },
    onDepartmentCreated() {
      this.showCreateModal = false
      this.fetchDepartments()
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

.departments-page {
  padding: 20px 0;
  font-family: 'Inter', sans-serif;
}

.departments-header {
  display: flex;
  align-items: center;
  gap: 20px;
  margin-bottom: 40px;
  padding: 28px 36px;
  background: linear-gradient(135deg, #1E3A5F 0%, #2563EB 100%);
  border-radius: 20px;
  box-shadow: 0 8px 24px rgba(37, 99, 235, 0.3);
}

.header-icon {
  width: 64px;
  height: 64px;
  background: rgba(255,255,255,0.15);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.header-text h2 {
  color: #FFFFFF;
  font-size: 28px;
  font-weight: 700;
  margin: 0 0 8px 0;
}

.header-text p {
  color: rgba(255,255,255,0.75);
  font-size: 15px;
  margin: 0;
}

.departments-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 20px;
}

.department-card {
  background: #FFFFFF;
  border-radius: 16px;
  padding: 28px 24px;
  cursor: pointer;
  transition: all 0.25s ease;
  border: 1px solid #E2E8F0;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
  position: relative;
  overflow: hidden;
}

.department-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #1E40AF, #2563EB);
  opacity: 0;
  transition: opacity 0.25s ease;
}

.department-card:hover::before {
  opacity: 1;
}

.department-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 30px rgba(37, 99, 235, 0.15);
  border-color: #BFDBFE;
}

.create-card {
  border-style: dashed;
  border-color: #BFDBFE;
  background: #F0F7FF;
}

.create-card:hover {
  border-style: solid;
  background: #FFFFFF;
}

.dept-icon {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 18px;
  box-shadow: 0 6px 16px rgba(37, 99, 235, 0.3);
}

.create-icon {
  background: #EFF6FF;
  box-shadow: none;
  border: 2px solid #BFDBFE;
}

.department-card h3 {
  color: #0F172A;
  font-size: 18px;
  font-weight: 700;
  margin: 0 0 8px 0;
}

.department-card p {
  color: #64748B;
  font-size: 13px;
  margin: 0;
}

.card-arrow {
  position: absolute;
  right: 20px;
  bottom: 20px;
  color: #CBD5E1;
  transition: all 0.2s ease;
}

.department-card:hover .card-arrow {
  color: #2563EB;
  transform: translateX(3px);
}

@media (max-width: 768px) {
  .departments-grid { grid-template-columns: 1fr; }
  .departments-header { flex-direction: column; text-align: center; padding: 24px; }
  .header-text h2 { font-size: 22px; }
}
</style>

<template>
  <div class="department-dashboard">
    <div class="department-header">
      <div class="header-title-group">
        <div class="dept-icon-badge">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
            <rect x="10" y="13" width="4" height="8"/>
          </svg>
        </div>
        <h2>{{ dashboardData.departmentName || getDepartmentName() }} Departmanı</h2>
      </div>
      <div class="header-actions">
        <button class="tab-btn" :class="{ active: !isAdminMenu }" @click="openTickets">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <rect x="3" y="4" width="18" height="18" rx="2"/>
            <path d="M8 9h8M8 13h6M8 17h4"/>
          </svg>
          Talepler
        </button>
        <button v-if="canAccessAdmin" class="tab-btn" :class="{ active: isAdminMenu }" @click="openAdmin">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="3"/>
            <path d="M19.07 4.93a10 10 0 010 14.14M4.93 4.93a10 10 0 000 14.14"/>
          </svg>
          Admin Menü
        </button>
      </div>
      <button @click="goBack" class="back-btn">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <polyline points="15 18 9 12 15 6"/>
        </svg>
        Departman Seçimine Dön
      </button>
    </div>
    
    <!-- Loading State -->
    <div v-if="loading" class="loading-state">
      <div class="loading-spinner-wrap">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="spin-svg">
          <path d="M21 12a9 9 0 11-6.219-8.56"/>
        </svg>
      </div>
      <p>Departman bilgileri yükleniyor...</p>
    </div>
    
    <!-- Error State -->
    <div v-else-if="error" class="error-state">
      <div class="error-icon-wrap">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/>
          <line x1="15" y1="9" x2="9" y2="15"/>
          <line x1="9" y1="9" x2="15" y2="15"/>
        </svg>
      </div>
      <h3>Hata Oluştu</h3>
      <p>{{ error }}</p>
      <button @click="fetchDashboardData" class="retry-btn">
        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <polyline points="23 4 23 10 17 10"/>
          <path d="M20.49 15a9 9 0 11-2.12-9.36L23 10"/>
        </svg>
        Tekrar Dene
      </button>
    </div>
    
    <!-- Content Switcher -->
    <div v-else>
      
      <!-- Admin Menu Content -->
      <div v-if="isAdminMenu" class="admin-content">
        <div class="admin-toolbar">
          <button
            v-for="mod in adminModules"
            :key="mod.id"
            class="admin-tool-btn"
            :class="{ active: activeAdminModuleId === mod.id }"
            @click="openAdminModule(mod.id)"
          >
            {{ mod.label }}
          </button>
        </div>

        <div class="admin-panel">
          <component
            v-if="activeAdminModule && activeAdminModule.component"
            :is="activeAdminModule.component"
            :department-code="departmentId"
            v-bind="activeAdminModule.props || {}"
          />
          <div v-else class="admin-placeholder">
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="#94A3B8" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
              <rect x="3" y="3" width="18" height="18" rx="2"/>
              <path d="M9 9h6M9 12h6M9 15h4"/>
            </svg>
            <h3>{{ activeAdminModule ? activeAdminModule.label : 'Admin Menüsü' }}</h3>
            <p>
              {{ activeAdminModule ? 'Bu modül için içerik eklenecek.' : 'Bir admin modülü seçin.' }}
            </p>
          </div>
        </div>
      </div>

      <!-- Department Dashboard Content -->
      <div v-else class="department-content">
        
        <!-- Department Stats -->
        <div class="department-stats">
          <div class="stat-card total">
            <div class="stat-icon total-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="2" y="3" width="20" height="14" rx="2"/>
                <path d="M8 21h8M12 17v4"/>
              </svg>
            </div>
            <div class="stat-content">
              <h4>Toplam Talep</h4>
              <div class="stat-number">{{ dashboardData.totalTickets || 0 }}</div>
            </div>
          </div>
          
          <div class="stat-card pending">
            <div class="stat-icon pending-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10"/>
                <polyline points="12 6 12 12 16 14"/>
              </svg>
            </div>
            <div class="stat-content">
              <h4>Bekleyen</h4>
              <div class="stat-number">{{ dashboardData.pendingTickets || 0 }}</div>
            </div>
          </div>
          
          <div class="stat-card accepted">
            <div class="stat-icon accepted-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12"/>
              </svg>
            </div>
            <div class="stat-content">
              <h4>Kabul Edildi</h4>
              <div class="stat-number">{{ dashboardData.acceptedTickets || 0 }}</div>
            </div>
          </div>
          
          <div class="stat-card waiting">
            <div class="stat-icon waiting-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10"/>
                <polyline points="12 6 12 12 16 14"/>
              </svg>
            </div>
            <div class="stat-content">
              <h4>Onay Bekliyor</h4>
              <div class="stat-number">{{ dashboardData.waitingApprovalTickets || 0 }}</div>
            </div>
          </div>
          
          <div class="stat-card approved">
            <div class="stat-icon approved-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M22 11.08V12a10 10 0 11-5.93-9.14"/>
                <polyline points="22 4 12 14.01 9 11.01"/>
              </svg>
            </div>
            <div class="stat-content">
              <h4>Tamamlandı</h4>
              <div class="stat-number">{{ dashboardData.approvedTickets || 0 }}</div>
            </div>
          </div>
        </div>
        
        <!-- Tickets Board -->
        <TicketBoard
          :api-endpoint="'/api/ticket/department-tickets'"
          :additional-params="{ departmentCode: departmentId }"
          :can-edit="true"
          :can-assign="false"
          :show-create-button="false"
          @assign-ticket="assignTicket"
          @ticket-updated="handleTicketUpdate"
        />

      </div>
    </div>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import TicketBoard from './TicketBoard.vue'
import UserManagement from './UserManagement.vue'
import CategoryManagement from './CategoryManagement.vue'

export default {
  name: 'DepartmentDashboard',
  components: {
    TicketBoard,
    UserManagement,
    CategoryManagement
  },
  props: {
    departmentId: {
      type: String,
      required: true,
      validator: function (value) {
        return value && value.trim().length > 0
      }
    }
  },
  data() {
    return {
      loading: false,
      error: null,
      isMounted: false,
      isAdminMenu: false,
      adminModules: [
        { id: 'overview', label: 'Genel Bakış', component: null },
        { id: 'users', label: 'Kullanıcı Yönetimi', component: 'UserManagement' },
        { id: 'categories', label: 'Kategori Yönetimi', component: 'CategoryManagement' },
        { id: 'departments', label: 'Departman Yönetimi', component: null }
      ],
      activeAdminModuleId: 'overview',
      dashboardData: {
        departmentCode: '',
        departmentName: '',
        totalTickets: 0,
        pendingTickets: 0,
        acceptedTickets: 0,
        waitingApprovalTickets: 0,
        approvedTickets: 0
      },
      departments: {
        'MANAGEMENT': 'Yönetim',
        'SALES': 'Satış',
        'IK': 'İnsan Kaynakları',
        'IT': 'Bilgi Teknolojileri'
      }
    }
  },
  mounted() {
    this.isMounted = true
    this.fetchDashboardData()
  },
  beforeUnmount() {
    this.isMounted = false
  },
  watch: {
    departmentId: {
      handler() {
        if (this.isMounted) {
          this.fetchDashboardData()
        }
      },
      immediate: false
    }
  },
  methods: {
    async fetchDashboardData() {
      if (!this.isMounted) return
      
      this.loading = true
      this.error = null
      
      try {
        const token = localStorage.getItem('authToken')
        if (!token) throw new Error('Oturum bulunamadı. Lütfen tekrar giriş yapın.')

        const response = await fetch(`/api/department/dashboard?departmentCode=${this.departmentId}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw await errorManager.createErrorFromResponse(response)

        const data = await response.json()
        
        if (this.isMounted) {
          this.dashboardData = {
            departmentCode: data.departmentCode || this.departmentId,
            departmentName: data.departmentName || this.getDepartmentName(),
            totalTickets: data.totalTickets || 0,
            pendingTickets: data.pendingTickets || 0,
            acceptedTickets: data.acceptedTickets || 0,
            waitingApprovalTickets: data.waitingApprovalTickets || 0,
            approvedTickets: data.approvedTickets || 0
          }
        }
        
      } catch (error) {
        if (this.isMounted) {
          errorManager.logError('Department Dashboard', error)
          this.error = error.message || 'Departman bilgileri yüklenirken bir hata oluştu.'
        }
      } finally {
        if (this.isMounted) this.loading = false
      }
    },

    getDepartmentName() {
      return this.departments[this.departmentId] || this.departmentId || 'Bilinmeyen Departman';
    },
    
    openAdmin() { this.isAdminMenu = true; },
    openTickets() { this.isAdminMenu = false; },
    openAdminModule(id) { this.activeAdminModuleId = id },
    goBack() {
      this.$emit('go-back')
      if (this.$router) {
        this.$router.push('/departments')
      }
    },
    assignTicket(ticketId) { console.log('Assigning ticket:', ticketId) },
    handleTicketUpdate(updatedData) { console.log('Ticket updated:', updatedData) }
  },
  computed: {
    activeAdminModule() {
      return this.adminModules.find(m => m.id === this.activeAdminModuleId)
    },
    
    canAccessAdmin() {
      const token = localStorage.getItem('authToken')
      if (!token) return false
      
      try {
        const base64Url = token.split('.')[1]
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        }).join(''))
        const payload = JSON.parse(jsonPayload)
        
        const departmentCodes = payload.departmentCode
        const priorities = payload.priority
        const deptCodes = Array.isArray(departmentCodes) ? departmentCodes : [departmentCodes]
        const priorityLevels = Array.isArray(priorities) ? priorities : [priorities]
        const deptIndex = deptCodes.findIndex(code => String(code).toUpperCase() === this.departmentId.toUpperCase())
        
        if (deptIndex >= 0 && deptIndex < priorityLevels.length) {
          const userPriority = parseInt(priorityLevels[deptIndex], 10)
          return userPriority >= 7
        }
        return false
      } catch (error) {
        return false
      }
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

.department-dashboard {
  padding: 0;
  width: 100%;
  font-family: 'Inter', sans-serif;
}

/* ===== HEADER ===== */
.department-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  padding: 24px 32px;
  background: #FFFFFF;
  border-radius: 16px;
  border: 1px solid #E2E8F0;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
}

.header-title-group {
  display: flex;
  align-items: center;
  gap: 14px;
}

.dept-icon-badge {
  width: 48px;
  height: 48px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.department-header h2 {
  color: #0F172A;
  font-size: 26px;
  font-weight: 700;
  margin: 0;
}

.back-btn {
  background: #F1F5F9;
  color: #475569;
  border: 1px solid #E2E8F0;
  padding: 10px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: 'Inter', sans-serif;
}

.back-btn:hover {
  background: #E2E8F0;
  color: #1E293B;
}

.header-actions {
  display: flex;
  gap: 8px;
  align-items: center;
}

.tab-btn {
  background: #F1F5F9;
  color: #64748B;
  border: 1px solid #E2E8F0;
  padding: 9px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  gap: 7px;
  font-family: 'Inter', sans-serif;
}

.tab-btn:hover {
  background: #E2E8F0;
  color: #1E293B;
}

.tab-btn.active {
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  border-color: transparent;
  box-shadow: 0 4px 10px rgba(37, 99, 235, 0.25);
}

/* ===== LOADING / ERROR ===== */
.loading-state {
  text-align: center;
  padding: 80px 20px;
}

.loading-spinner-wrap {
  margin-bottom: 20px;
  display: flex;
  justify-content: center;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.spin-svg { animation: spin 1s linear infinite; }

.loading-state p {
  color: #64748B;
  font-size: 16px;
  margin: 0;
}

.error-state {
  text-align: center;
  padding: 60px 20px;
  background: #FEF2F2;
  border-radius: 16px;
  border: 1px solid #FECACA;
}

.error-icon-wrap {
  margin-bottom: 16px;
  display: flex;
  justify-content: center;
}

.error-state h3 {
  color: #DC2626;
  font-size: 22px;
  font-weight: 600;
  margin: 0 0 12px 0;
}

.error-state p {
  color: #B91C1C;
  font-size: 15px;
  margin: 0 0 24px 0;
}

.retry-btn {
  background: #EF4444;
  color: white;
  border: none;
  padding: 10px 22px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  font-family: 'Inter', sans-serif;
}

.retry-btn:hover {
  background: #DC2626;
  transform: translateY(-1px);
}

/* ===== STATS ===== */
.department-stats {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 16px;
  margin-bottom: 24px;
}

.stat-card {
  background: #FFFFFF;
  border-radius: 14px;
  padding: 20px;
  display: flex;
  align-items: center;
  gap: 16px;
  border: 1px solid #E2E8F0;
  box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  transition: all 0.25s ease;
  position: relative;
  overflow: hidden;
}

.stat-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
}

.stat-card.total::before { background: linear-gradient(90deg, #1E40AF, #3B82F6); }
.stat-card.pending::before { background: linear-gradient(90deg, #D97706, #F59E0B); }
.stat-card.accepted::before { background: linear-gradient(90deg, #059669, #10B981); }
.stat-card.waiting::before { background: linear-gradient(90deg, #0284C7, #38BDF8); }
.stat-card.approved::before { background: linear-gradient(90deg, #7C3AED, #A78BFA); }

.stat-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(0,0,0,0.1);
}

.stat-icon {
  width: 52px;
  height: 52px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.total-icon { background: linear-gradient(135deg, #1E40AF, #3B82F6); }
.pending-icon { background: linear-gradient(135deg, #D97706, #F59E0B); }
.accepted-icon { background: linear-gradient(135deg, #059669, #10B981); }
.waiting-icon { background: linear-gradient(135deg, #0284C7, #38BDF8); }
.approved-icon { background: linear-gradient(135deg, #7C3AED, #A78BFA); }

.stat-content h4 {
  color: #64748B;
  font-size: 12px;
  font-weight: 600;
  margin: 0 0 6px 0;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.stat-number {
  color: #0F172A;
  font-size: 30px;
  font-weight: 800;
  line-height: 1;
}

/* ===== ADMIN ===== */
.admin-content { padding: 0; }

.admin-toolbar {
  display: flex;
  gap: 8px;
  margin-bottom: 16px;
  flex-wrap: wrap;
}

.admin-tool-btn {
  background: #F1F5F9;
  color: #64748B;
  border: 1px solid #E2E8F0;
  padding: 9px 16px;
  border-radius: 8px;
  font-weight: 600;
  font-size: 13px;
  transition: all 0.2s ease;
  cursor: pointer;
  font-family: 'Inter', sans-serif;
}

.admin-tool-btn:hover {
  background: #E2E8F0;
  color: #1E293B;
}

.admin-tool-btn.active {
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  border-color: transparent;
  box-shadow: 0 4px 10px rgba(37, 99, 235, 0.25);
}

.admin-panel {
  background: #FFFFFF;
  border: 1px solid #E2E8F0;
  border-radius: 16px;
  padding: 24px;
  min-height: 280px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.05);
}

.admin-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 30px;
  text-align: center;
  color: #94A3B8;
  gap: 12px;
}

.admin-placeholder h3 {
  color: #475569;
  font-size: 18px;
  font-weight: 600;
  margin: 0;
}

.admin-placeholder p {
  color: #94A3B8;
  font-size: 14px;
  margin: 0;
}

/* ===== RESPONSIVE ===== */
@media (max-width: 1200px) {
  .department-stats { grid-template-columns: repeat(3, 1fr); }
}

@media (max-width: 768px) {
  .department-header {
    flex-direction: column;
    gap: 16px;
    padding: 20px;
  }
  .department-header h2 { font-size: 20px; }
  .department-stats { grid-template-columns: repeat(2, 1fr); }
  .stat-card { padding: 16px; }
  .stat-icon { width: 44px; height: 44px; }
  .stat-number { font-size: 24px; }
}
</style>

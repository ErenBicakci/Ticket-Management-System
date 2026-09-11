<template>
  <div class="content-area-wrapper">
    <div v-if="currentPage === 'dashboard'" class="dashboard-content">
      <div class="empty-dashboard">
      </div>
    </div>
    
    <div v-else-if="currentPage === 'new-ticket'" class="ticket-content">
      <TalepForm
        @go-back="goToDashboard"
        @ticket-created="onTicketCreated"
      />
    </div>
    
    <div v-else-if="currentPage === 'my-tickets'" class="tickets-content">
      <MyTickets
        :user-info="userInfo"
        @navigate="handleNavigation"
      />
    </div>
    
    <div v-else-if="currentPage === 'user-stats'" class="user-stats-content">
      <UserStats />
    </div>

    <div v-else-if="currentPage === 'departments'" class="departments-content">
      <DepartmentsPage
        @select-department="onDepartmentSelected"
      />
    </div>
    
    <div v-else-if="currentPage.startsWith('dept-') && getDepartmentId()" class="department-dashboard-content">
      <DepartmentDashboard
        :department-id="getDepartmentId()"
        @go-back="goToDepartments"
      />
    </div>
    
    <div v-else class="page-not-found">
      <h2>Sayfa Bulunamadı</h2>
      <p>Aradığınız sayfa mevcut değil.</p>
      <button @click="goToDashboard" class="back-to-dashboard-btn">
        Ana Sayfaya Dön
      </button>
    </div>
  </div>
</template>

<script>
import TalepForm from './TalepForm.vue'
import MyTickets from './MyTickets.vue'
import DepartmentsPage from './DepartmentsPage.vue'
import DepartmentDashboard from './DepartmentDashboard.vue'
import UserStats from './UserStats.vue'

export default {
  name: 'ContentArea',
  components: {
    TalepForm,
    MyTickets,
    DepartmentsPage,
    DepartmentDashboard,
    UserStats
  },
  props: {
    currentPage: {
      type: String,
      required: true
    },
    userInfo: {
      type: Object,
      default: null
    }
  },
  methods: {
    handleNavigation(page) {
      this.$emit('navigate', page);
    },
    
    goToDashboard() {
      this.$emit('navigate', 'dashboard');
    },
    
    goToDepartments() {
      this.$emit('navigate', 'departments');
    },
    
    onTicketCreated(ticketData) {
      this.$emit('ticket-created', ticketData);
    },
    
    onDepartmentSelected(departmentId) {
      this.$emit('select-department', departmentId);
    },
    
    getDepartmentId() {
      const deptMatch = this.currentPage.match(/dept-([A-Za-z0-9_-]+)/);
      if (deptMatch && deptMatch[1]) {
        return deptMatch[1];
      }
      return null;
    }
  }
}
</script>

<style scoped>
.content-area-wrapper {
  width: 100%;
  min-height: 100%;
}

.dashboard-content {
  min-height: 400px;
}

.empty-dashboard {
  min-height: 400px;
}

.ticket-content {
  width: 100%;
}

.tickets-content {
  width: 100%;
}

.departments-content {
  width: 100%;
}

.department-dashboard-content {
  width: 100%;
}

.page-not-found {
  text-align: center;
  padding: 100px 20px;
}

.page-not-found h2 {
  color: #2c5f5f;
  font-size: 32px;
  font-weight: 700;
  margin-bottom: 20px;
}

.page-not-found p {
  color: #5a6b6b;
  font-size: 18px;
  margin-bottom: 30px;
}

.back-to-dashboard-btn {
  background: linear-gradient(135deg, #2c5f5f 0%, #1a3a3a 100%);
  color: #f8f6f2;
  border: none;
  padding: 15px 30px;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.back-to-dashboard-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(44, 95, 95, 0.3);
}

@media (max-width: 768px) {
  .page-not-found {
    padding: 60px 20px;
  }
  
  .page-not-found h2 {
    font-size: 24px;
  }
  
  .page-not-found p {
    font-size: 16px;
  }
}
</style>

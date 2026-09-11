<template>
  <div class="user-management">
    <div class="user-management-header">
      <div class="header-titles">
        <h3>Kullanıcı Yönetimi</h3>
        <p>Bu departmana atanmış kullanıcıları ve yetki seviyelerini yönetin.</p>
      </div>
      <div class="header-actions">
        <button @click="openAddUser" class="btn btn-add">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="12" y1="5" x2="12" y2="19"/>
            <line x1="5" y1="12" x2="19" y2="12"/>
          </svg>
          <span>Yeni Kullanıcı Ekle</span>
        </button>
        <button @click="refreshUsers" class="btn btn-refresh" :disabled="loading">
          <svg v-if="loading" class="spin-svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12a9 9 0 11-6.219-8.56"/>
          </svg>
          <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="23 4 23 10 17 10"/>
            <path d="M20.49 15a9 9 0 11-2.12-9.36L23 10"/>
          </svg>
          <span>Yenile</span>
        </button>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="state-container">
      <svg class="spin-svg" width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <p>Kullanıcılar yükleniyor...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="state-container error-state">
      <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <circle cx="12" cy="12" r="10"/>
        <line x1="15" y1="9" x2="9" y2="15"/>
        <line x1="9" y1="9" x2="15" y2="15"/>
      </svg>
      <h4>Hata Oluştu</h4>
      <p>{{ error }}</p>
      <button @click="fetchUsers" class="btn btn-refresh">Tekrar Dene</button>
    </div>

    <!-- Empty State -->
    <div v-else-if="users.length === 0" class="state-container empty-state">
      <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#CBD5E1" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
        <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/>
        <path d="M23 21v-2a4 4 0 00-3-3.87"/><path d="M16 3.13a4 4 0 010 7.75"/>
      </svg>
      <h4>Kullanıcı Bulunamadı</h4>
      <p>Bu departmanda henüz kayıtlı bir kullanıcı bulunmuyor.</p>
      <button @click="openAddUser" class="btn btn-add">Kullanıcı Ekle</button>
    </div>

    <!-- Users List -->
    <div v-else class="users-grid">
      <UserCard
        v-for="user in users"
        :key="user.username"
        :user="user"
        :is-removing="removingUser === user.username"
        @remove-user="handleRemoveUser"
      />
    </div>

    <!-- Modals -->
    <UserSearchModal
      :visible="showAddModal"
      :existing-usernames="existingUsernames"
      @close="showAddModal = false"
      @select-user="handleUserSelectedForRole"
    />

    <RoleAssignmentModal
      :visible="showRoleModal"
      :user="selectedUser"
      :is-submitting="isAssigningRole"
      @close="showRoleModal = false"
      @confirm="handleRoleAssignmentConfirm"
    />
  </div>
</template>

<script>
import { useToastStore } from '../stores/toast.js'
import UserCard from './user/UserCard.vue'
import UserSearchModal from './user/UserSearchModal.vue'
import RoleAssignmentModal from './user/RoleAssignmentModal.vue'

export default {
  name: 'UserManagement',
  components: {
    UserCard,
    UserSearchModal,
    RoleAssignmentModal
  },
  props: {
    departmentCode: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      users: [],
      loading: false,
      error: null,
      removingUser: null,
      showAddModal: false,
      showRoleModal: false,
      selectedUser: null,
      isAssigningRole: false
    }
  },
  computed: {
    toastStore() {
      return useToastStore()
    },
    existingUsernames() {
      return this.users.map(u => u.username)
    }
  },
  mounted() {
    this.fetchUsers()
  },
  watch: {
    departmentCode() {
      this.fetchUsers()
    }
  },
  methods: {
    async fetchUsers() {
      this.loading = true
      this.error = null
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/user/department-users?departmentCode=${this.departmentCode}`, {
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) throw new Error('Departman kullanıcıları yüklenemedi.')
        const data = await response.json()
        this.users = Array.isArray(data) ? data : []
      } catch (err) {
        this.error = err.message || 'Kullanıcılar alınamadı.'
      } finally {
        this.loading = false
      }
    },

    refreshUsers() {
      this.fetchUsers()
    },

    openAddUser() {
      this.showAddModal = true
    },

    handleUserSelectedForRole(user) {
      this.showAddModal = false
      this.selectedUser = user
      this.showRoleModal = true
    },

    async handleRoleAssignmentConfirm({ user, priorityLevel, roleDescription }) {
      this.isAssigningRole = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/department/assign-role', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          },
          body: JSON.stringify({
            username: user.username,
            departmentCode: this.departmentCode,
            priorityLevel: Number(priorityLevel),
            roleDescription
          })
        })

        if (!response.ok) {
          const errData = await response.json().catch(() => null)
          throw new Error(errData?.message || 'Rol atama işlemi başarısız.')
        }

        this.showRoleModal = false
        this.toastStore.success(`@${user.username} kullanıcısı başarıyla eklendi.`, 'Rol Atandı')
        this.fetchUsers()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isAssigningRole = false
      }
    },

    async handleRemoveUser(user) {
      if (!confirm(`@${user.username} kullanıcısını bu departmandan kaldırmak istediğinize emin misiniz?`)) {
        return
      }

      this.removingUser = user.username
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/department/${this.departmentCode}/users/${user.username}`, {
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) throw new Error('Kullanıcı kaldırılamadı.')

        this.toastStore.success(`@${user.username} kullanıcısı kaldırıldı.`, 'Başarılı')
        this.fetchUsers()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.removingUser = null
      }
    }
  }
}
</script>

<style scoped>
.user-management {
  display: flex;
  flex-direction: column;
  gap: 20px;
  font-family: 'Inter', sans-serif;
}

.user-management-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 16px;
  background: #FFFFFF;
  padding: 20px 24px;
  border-radius: 14px;
  border: 1px solid #E2E8F0;
}

.header-titles h3 {
  font-size: 18px;
  font-weight: 700;
  color: #0F172A;
  margin: 0 0 4px 0;
}

.header-titles p {
  font-size: 13px;
  color: #64748B;
  margin: 0;
}

.header-actions {
  display: flex;
  gap: 10px;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 9px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
}

.btn-add {
  background: #2563EB;
  color: white;
}
.btn-add:hover {
  background: #1D4ED8;
}

.btn-refresh {
  background: #F1F5F9;
  color: #475569;
  border: 1px solid #E2E8F0;
}
.btn-refresh:hover:not(:disabled) {
  background: #E2E8F0;
  color: #0F172A;
}

.state-container {
  text-align: center;
  padding: 48px 20px;
  background: #FFFFFF;
  border-radius: 14px;
  border: 1px solid #E2E8F0;
  color: #64748B;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.users-grid {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.spin-svg {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>

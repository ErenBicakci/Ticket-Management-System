<template>
  <div v-if="visible" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-card">
      <div class="modal-card-header">
        <h3>Kullanıcıya Ata</h3>
        <button class="modal-close-btn" @click="$emit('close')">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>

      <div class="modal-card-body">
        <!-- Search filter -->
        <div class="search-wrap">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Kullanıcı adı veya isim ile filtrele..."
            class="user-filter-input"
          />
        </div>

        <div v-if="loading" class="assign-state">
          <svg class="spin-svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12a9 9 0 11-6.219-8.56"/>
          </svg>
          <p>Kullanıcılar yükleniyor...</p>
        </div>

        <div v-else-if="error" class="assign-state error">
          <p>{{ error }}</p>
          <button @click="$emit('retry')" class="retry-btn">Tekrar Dene</button>
        </div>

        <div v-else-if="filteredUsers.length === 0" class="assign-state empty">
          <p>Eşleşen kullanıcı bulunamadı.</p>
        </div>

        <div v-else class="users-list">
          <div
            v-for="user in filteredUsers"
            :key="user.username"
            class="user-row"
            @click="$emit('select-user', user)"
          >
            <div class="user-avatar-circle">
              {{ (user.fullName || user.username || '?').charAt(0).toUpperCase() }}
            </div>
            <div class="user-row-info">
              <span class="user-row-name">{{ user.fullName || user.username }}</span>
              <span class="user-row-username">@{{ user.username }}</span>
            </div>
            <button class="btn-assign-action">
              <span>Seç ve Ata</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TicketUserAssignModal',
  props: {
    visible: {
      type: Boolean,
      default: false
    },
    users: {
      type: Array,
      default: () => []
    },
    loading: {
      type: Boolean,
      default: false
    },
    error: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      searchQuery: ''
    }
  },
  computed: {
    filteredUsers() {
      if (!this.searchQuery.trim()) return this.users
      const q = this.searchQuery.toLowerCase()
      return this.users.filter(u => {
        const name = (u.fullName || '').toLowerCase()
        const username = (u.username || '').toLowerCase()
        return name.includes(q) || username.includes(q)
      })
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99999;
}

.modal-card {
  background: #FFFFFF;
  border-radius: 16px;
  width: 100%;
  max-width: 460px;
  max-height: 85vh;
  display: flex;
  flex-direction: column;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.2);
  overflow: hidden;
}

.modal-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 22px;
  border-bottom: 1px solid #E2E8F0;
}

.modal-card-header h3 {
  font-size: 17px;
  font-weight: 700;
  color: #0F172A;
  margin: 0;
}

.modal-close-btn {
  background: #F1F5F9;
  border: none;
  border-radius: 8px;
  padding: 6px;
  cursor: pointer;
  color: #64748B;
  display: flex;
}

.modal-card-body {
  padding: 18px 22px;
  display: flex;
  flex-direction: column;
  gap: 14px;
  overflow-y: auto;
}

.search-wrap {
  width: 100%;
}

.user-filter-input {
  width: 100%;
  padding: 9px 12px;
  border: 1.5px solid #CBD5E1;
  border-radius: 8px;
  font-size: 13px;
  font-family: inherit;
}

.user-filter-input:focus {
  outline: none;
  border-color: #2563EB;
}

.assign-state {
  text-align: center;
  padding: 30px;
  color: #64748B;
  font-size: 13px;
}

.users-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  max-height: 320px;
  overflow-y: auto;
}

.user-row {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 12px;
  border-radius: 10px;
  border: 1px solid #E2E8F0;
  cursor: pointer;
  transition: all 0.2s ease;
}

.user-row:hover {
  background: #F8FAFC;
  border-color: #2563EB;
}

.user-avatar-circle {
  width: 34px;
  height: 34px;
  border-radius: 50%;
  background: #2563EB;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 13px;
}

.user-row-info {
  flex: 1;
  min-width: 0;
}

.user-row-name {
  display: block;
  font-size: 13px;
  font-weight: 700;
  color: #0F172A;
}

.user-row-username {
  display: block;
  font-size: 11px;
  color: #64748B;
}

.btn-assign-action {
  background: #EFF6FF;
  color: #2563EB;
  border: 1px solid #DBEAFE;
  border-radius: 6px;
  padding: 6px 12px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.user-row:hover .btn-assign-action {
  background: #2563EB;
  color: #FFFFFF;
}

.spin-svg {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>

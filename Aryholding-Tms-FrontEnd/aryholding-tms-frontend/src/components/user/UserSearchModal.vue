<template>
  <div v-if="visible" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-box">
      <div class="modal-header">
        <h4>Yeni Kullanıcı Ekle</h4>
        <button class="modal-close" @click="$emit('close')">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>

      <div class="modal-body">
        <div class="search-row">
          <input
            type="text"
            v-model.trim="usernameQuery"
            @keyup.enter="search"
            placeholder="Kullanıcı adı ile ara (örn. eren)"
            class="search-input"
          />
          <button class="search-btn" @click="search" :disabled="loading || !usernameQuery">
            <svg v-if="loading" class="spin-svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M21 12a9 9 0 11-6.219-8.56"/>
            </svg>
            <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="11" cy="11" r="8"/>
              <line x1="21" y1="21" x2="16.65" y2="16.65"/>
            </svg>
            <span>Ara</span>
          </button>
        </div>

        <div v-if="error" class="search-feedback error">{{ error }}</div>
        <div v-if="loading" class="search-feedback loading">Aranıyor...</div>

        <!-- Found User Result Card -->
        <div v-else-if="foundUser" class="found-user-card">
          <div class="found-user-info">
            <div class="avatar-circle">
              {{ getUserInitials(foundUser.fullName || foundUser.username) }}
            </div>
            <div class="user-text">
              <div class="name">{{ foundUser.fullName || foundUser.username }}</div>
              <div class="username">@{{ foundUser.username }}</div>
              <div v-if="foundUser.email" class="email">{{ foundUser.email }}</div>
            </div>
          </div>
          <button
            class="add-role-btn"
            @click="$emit('select-user', foundUser)"
            :disabled="isAlreadyInDept(foundUser)"
          >
            {{ isAlreadyInDept(foundUser) ? 'Zaten Departmanda' : 'Departmanda Rol Ekle' }}
          </button>
        </div>

        <div v-else-if="searched && !loading" class="search-feedback empty">
          Kullanıcı bulunamadı.
        </div>
        <div v-else class="search-hint">
          Aramak için kullanıcı adını yazın ve Enter'a basın.
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UserSearchModal',
  props: {
    visible: { type: Boolean, default: false },
    existingUsernames: { type: Array, default: () => [] }
  },
  data() {
    return {
      usernameQuery: '',
      loading: false,
      error: null,
      foundUser: null,
      searched: false
    }
  },
  watch: {
    visible(val) {
      if (val) {
        this.usernameQuery = ''
        this.foundUser = null
        this.error = null
        this.searched = false
      }
    }
  },
  methods: {
    getUserInitials(name) {
      if (!name) return '?'
      return name.charAt(0).toUpperCase()
    },
    isAlreadyInDept(user) {
      if (!user || !user.username) return false
      return this.existingUsernames.includes(user.username)
    },
    async search() {
      if (!this.usernameQuery.trim()) return

      this.loading = true
      this.error = null
      this.foundUser = null
      this.searched = true

      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/user/search?username=${encodeURIComponent(this.usernameQuery.trim())}`, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error('Kullanıcı bulunamadı.')
        const data = await response.json()
        if (data && data.username) {
          this.foundUser = data
        } else if (Array.isArray(data) && data.length > 0) {
          this.foundUser = data[0]
        } else {
          this.foundUser = null
        }
      } catch (err) {
        this.error = err.message || 'Arama sırasında hata oluştu.'
      } finally {
        this.loading = false
      }
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

.modal-box {
  background: #FFFFFF;
  border-radius: 16px;
  width: 100%;
  max-width: 480px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.2);
  overflow: hidden;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 22px;
  border-bottom: 1px solid #E2E8F0;
}

.modal-header h4 {
  font-size: 16px;
  font-weight: 700;
  color: #0F172A;
  margin: 0;
}

.modal-close {
  background: #F1F5F9;
  border: none;
  border-radius: 8px;
  padding: 6px;
  cursor: pointer;
  color: #64748B;
  display: flex;
}

.modal-body {
  padding: 22px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.search-row {
  display: flex;
  gap: 8px;
}

.search-input {
  flex: 1;
  padding: 10px 14px;
  border: 1.5px solid #CBD5E1;
  border-radius: 8px;
  font-size: 13px;
  font-family: inherit;
}

.search-input:focus {
  outline: none;
  border-color: #2563EB;
}

.search-btn {
  background: #2563EB;
  color: white;
  border: none;
  border-radius: 8px;
  padding: 10px 16px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 6px;
}

.search-feedback {
  text-align: center;
  padding: 14px;
  font-size: 13px;
  border-radius: 8px;
}

.search-feedback.error {
  background: #FEF2F2;
  color: #DC2626;
}

.search-feedback.loading {
  color: #2563EB;
}

.search-feedback.empty {
  color: #94A3B8;
}

.search-hint {
  text-align: center;
  font-size: 12px;
  color: #94A3B8;
}

.found-user-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #F8FAFC;
  border: 1.5px solid #DBEAFE;
  border-radius: 12px;
  padding: 14px 16px;
  gap: 12px;
}

.found-user-info {
  display: flex;
  align-items: center;
  gap: 12px;
}

.avatar-circle {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #2563EB;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 14px;
}

.user-text .name {
  font-size: 14px;
  font-weight: 700;
  color: #0F172A;
}

.user-text .username {
  font-size: 12px;
  color: #64748B;
}

.user-text .email {
  font-size: 11px;
  color: #94A3B8;
}

.add-role-btn {
  background: #10B981;
  color: white;
  border: none;
  border-radius: 8px;
  padding: 8px 14px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
}

.add-role-btn:disabled {
  background: #CBD5E1;
  cursor: not-allowed;
}

.spin-svg {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>

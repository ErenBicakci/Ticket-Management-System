<template>
  <div v-if="visible" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-box role-modal">
      <div class="modal-header">
        <h4>Departmanda Rol Ekle</h4>
        <button class="modal-close" @click="$emit('close')">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>

      <div class="modal-body">
        <!-- User Preview -->
        <div v-if="user" class="selected-user-card">
          <div class="avatar-lg">
            {{ getUserInitials(user.fullName || user.username) }}
          </div>
          <div class="user-meta">
            <div class="name">{{ user.fullName || user.username }}</div>
            <div class="sub">@{{ user.username }} <span v-if="user.email">· {{ user.email }}</span></div>
          </div>
        </div>

        <!-- Priority Options -->
        <div class="priority-selection">
          <h5>Öncelik / Yetki Seviyesi Seçin:</h5>
          <div class="priority-grid">
            <div
              v-for="level in availableLevels"
              :key="level"
              class="priority-card"
              :class="{ selected: selectedLevel === level }"
              @click="selectedLevel = level"
            >
              <div class="priority-header-row">
                <span class="rank-name">{{ getRankName(level) }}</span>
                <span class="level-badge">Seviye {{ level }}</span>
              </div>
              <p class="rank-desc">{{ getRankDescription(level) }}</p>
            </div>
          </div>
        </div>

        <!-- Role Description Input -->
        <div class="role-desc-group">
          <label>Rol Açıklaması (Opsiyonel):</label>
          <input
            v-model="roleDescription"
            type="text"
            placeholder="Örn: Kıdemli Yazılım Geliştirici, Destek Uzmanı"
            class="role-desc-input"
          />
        </div>

        <div class="modal-actions">
          <button class="btn-cancel" @click="$emit('close')" :disabled="isSubmitting">İptal</button>
          <button
            class="btn-confirm"
            @click="handleConfirm"
            :disabled="!selectedLevel || isSubmitting"
          >
            <span v-if="isSubmitting">Rol Ekleniyor...</span>
            <span v-else>Onayla ve Ekle</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'RoleAssignmentModal',
  props: {
    visible: { type: Boolean, default: false },
    user: { type: Object, default: null },
    isSubmitting: { type: Boolean, default: false }
  },
  data() {
    return {
      selectedLevel: 1,
      roleDescription: '',
      availableLevels: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    }
  },
  watch: {
    visible(val) {
      if (val) {
        this.selectedLevel = 1
        this.roleDescription = ''
      }
    }
  },
  methods: {
    getUserInitials(name) {
      if (!name) return '?'
      return name.charAt(0).toUpperCase()
    },
    getRankName(level) {
      const names = {
        1: 'Stajyer / Yeni Başlayan',
        2: 'Yardımcı Uzman',
        3: 'Uzman',
        4: 'Kıdemli Uzman',
        5: 'Takım Lideri',
        6: 'Birim Yöneticisi',
        7: 'Departman Müdürü',
        8: 'Direktör',
        9: 'Genel Müdür Yardımcısı',
        10: 'Genel Müdür / Yönetim'
      }
      return names[level] || `Seviye ${level}`
    },
    getRankDescription(level) {
      if (level >= 7) return 'Departman içi onay yetkisi ve admin erişimi.'
      if (level >= 4) return 'Gelişmiş talep yönetimi ve üstlenme yetkisi.'
      return 'Standart talep oluşturma ve takip yetkisi.'
    },
    handleConfirm() {
      this.$emit('confirm', {
        user: this.user,
        priorityLevel: this.selectedLevel,
        roleDescription: this.roleDescription.trim() || this.getRankName(this.selectedLevel)
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

.modal-box {
  background: #FFFFFF;
  border-radius: 16px;
  width: 100%;
  max-width: 540px;
  max-height: 85vh;
  display: flex;
  flex-direction: column;
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
  overflow-y: auto;
}

.selected-user-card {
  display: flex;
  align-items: center;
  gap: 12px;
  background: #EFF6FF;
  border: 1px solid #DBEAFE;
  border-radius: 12px;
  padding: 12px 14px;
}

.avatar-lg {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  background: #2563EB;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 15px;
}

.user-meta .name {
  font-weight: 700;
  color: #1E40AF;
  font-size: 14px;
}

.user-meta .sub {
  font-size: 12px;
  color: #64748B;
}

.priority-selection h5 {
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
  color: #64748B;
  margin: 0 0 10px 0;
}

.priority-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
  max-height: 220px;
  overflow-y: auto;
  padding-right: 4px;
}

.priority-card {
  border: 1.5px solid #E2E8F0;
  border-radius: 10px;
  padding: 10px 12px;
  cursor: pointer;
  transition: all 0.15s ease;
}

.priority-card:hover {
  background: #F8FAFC;
  border-color: #CBD5E1;
}

.priority-card.selected {
  background: #EFF6FF;
  border-color: #2563EB;
}

.priority-header-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2px;
}

.rank-name {
  font-size: 13px;
  font-weight: 700;
  color: #0F172A;
}

.level-badge {
  font-size: 11px;
  font-weight: 700;
  background: #E2E8F0;
  color: #475569;
  padding: 2px 6px;
  border-radius: 4px;
}

.priority-card.selected .level-badge {
  background: #2563EB;
  color: white;
}

.rank-desc {
  font-size: 11px;
  color: #64748B;
  margin: 0;
}

.role-desc-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.role-desc-group label {
  font-size: 12px;
  font-weight: 600;
  color: #475569;
}

.role-desc-input {
  padding: 9px 12px;
  border: 1.5px solid #CBD5E1;
  border-radius: 8px;
  font-size: 13px;
  font-family: inherit;
}

.role-desc-input:focus {
  outline: none;
  border-color: #2563EB;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 8px;
}

.btn-cancel {
  background: #F1F5F9;
  border: 1px solid #E2E8F0;
  border-radius: 8px;
  padding: 9px 16px;
  font-size: 13px;
  font-weight: 600;
  color: #475569;
  cursor: pointer;
}

.btn-confirm {
  background: #2563EB;
  border: none;
  border-radius: 8px;
  padding: 9px 18px;
  font-size: 13px;
  font-weight: 600;
  color: white;
  cursor: pointer;
}

.btn-confirm:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>

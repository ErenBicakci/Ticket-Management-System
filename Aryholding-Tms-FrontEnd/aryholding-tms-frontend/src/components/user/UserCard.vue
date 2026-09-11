<template>
  <div class="user-card">
    <div class="user-header">
      <div class="user-avatar">
        <span>{{ getUserInitials(user.fullName || user.username) }}</span>
      </div>
      <div class="user-info">
        <h4 class="user-name">{{ user.fullName || user.username }}</h4>
        <p class="user-username">@{{ user.username }}</p>
        <p v-if="user.employeeId" class="user-employee-id">ID: {{ user.employeeId }}</p>
      </div>
      <div v-if="user.email" class="user-email">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/>
          <polyline points="22,6 12,13 2,6"/>
        </svg>
        <span>{{ user.email }}</span>
      </div>
      <div class="user-actions">
        <button
          v-if="canRemove"
          @click="$emit('remove-user', user)"
          class="remove-user-btn"
          :disabled="isRemoving"
        >
          <svg v-if="isRemoving" class="spin-icon" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12a9 9 0 11-6.219-8.56"/>
          </svg>
          <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="3 6 5 6 21 6"/>
            <path d="M19 6l-1 14a2 2 0 01-2 2H8a2 2 0 01-2-2L5 6"/>
            <path d="M10 11v6"/>
            <path d="M14 11v6"/>
            <path d="M9 6V4a1 1 0 011-1h4a1 1 0 011 1v2"/>
          </svg>
          <span>{{ isRemoving ? 'Kaldırılıyor...' : 'Kullanıcıyı Kaldır' }}</span>
        </button>
      </div>
    </div>

    <div v-if="user.departmentRoles && user.departmentRoles.length > 0" class="user-roles">
      <h5>Departman Rolleri:</h5>
      <div class="roles-grid">
        <div
          v-for="role in user.departmentRoles"
          :key="`${role.departmentCode}-${role.roleDescription || role.priorityLevel}`"
          class="role-card"
        >
          <div class="role-header">
            <span class="department-code">{{ role.departmentCode }}</span>
            <span class="priority-badge" :class="getPriorityClass(role.priorityLevel)">
              {{ role.priorityName || `Seviye ${role.priorityLevel}` }}
            </span>
          </div>
          <div class="role-details">
            <p v-if="role.departmentName" class="department-name">{{ role.departmentName }}</p>
            <p v-if="role.roleDescription" class="role-description">{{ role.roleDescription }}</p>
            <p class="priority-level">Öncelik Seviyesi: {{ role.priorityLevel }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UserCard',
  props: {
    user: { type: Object, required: true },
    canRemove: { type: Boolean, default: true },
    isRemoving: { type: Boolean, default: false }
  },
  methods: {
    getUserInitials(name) {
      if (!name) return '?'
      return name.charAt(0).toUpperCase()
    },
    getPriorityClass(level) {
      const num = parseInt(level, 10)
      if (num >= 8) return 'priority-high'
      if (num >= 5) return 'priority-mid'
      return 'priority-low'
    }
  }
}
</script>

<style scoped>
.user-card {
  background: #FFFFFF;
  border-radius: 14px;
  border: 1px solid #E2E8F0;
  padding: 18px 20px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
  display: flex;
  flex-direction: column;
  gap: 14px;
  transition: all 0.2s ease;
}

.user-card:hover {
  border-color: #CBD5E1;
  box-shadow: 0 4px 12px rgba(15, 23, 42, 0.06);
}

.user-header {
  display: flex;
  align-items: center;
  gap: 14px;
  flex-wrap: wrap;
}

.user-avatar {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  background: linear-gradient(135deg, #1E3A5F, #2563EB);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 16px;
  flex-shrink: 0;
}

.user-info {
  flex: 1;
  min-width: 160px;
}

.user-name {
  font-size: 15px;
  font-weight: 700;
  color: #0F172A;
  margin: 0 0 2px 0;
}

.user-username {
  font-size: 12px;
  color: #64748B;
  margin: 0;
}

.user-employee-id {
  font-size: 11px;
  color: #94A3B8;
  margin: 2px 0 0 0;
}

.user-email {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: #64748B;
  background: #F8FAFC;
  padding: 6px 12px;
  border-radius: 8px;
  border: 1px solid #E2E8F0;
}

.user-actions {
  margin-left: auto;
}

.remove-user-btn {
  background: rgba(239, 68, 68, 0.08);
  color: #EF4444;
  border: 1px solid rgba(239, 68, 68, 0.2);
  border-radius: 8px;
  padding: 8px 14px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s ease;
}

.remove-user-btn:hover:not(:disabled) {
  background: #EF4444;
  color: #FFFFFF;
}

.remove-user-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.user-roles {
  border-top: 1px solid #F1F5F9;
  padding-top: 12px;
}

.user-roles h5 {
  font-size: 11px;
  font-weight: 700;
  text-transform: uppercase;
  color: #94A3B8;
  letter-spacing: 0.5px;
  margin: 0 0 8px 0;
}

.roles-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.role-card {
  background: #F8FAFC;
  border: 1px solid #E2E8F0;
  border-radius: 10px;
  padding: 10px 14px;
  font-size: 12px;
  min-width: 180px;
}

.role-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 6px;
}

.department-code {
  font-weight: 700;
  color: #1E3A5F;
}

.priority-badge {
  font-size: 10px;
  font-weight: 700;
  padding: 2px 7px;
  border-radius: 6px;
}

.priority-high {
  background: #FEF2F2;
  color: #DC2626;
  border: 1px solid #FCA5A5;
}

.priority-mid {
  background: #FFFBEB;
  color: #D97706;
  border: 1px solid #FCD34D;
}

.priority-low {
  background: #EFF6FF;
  color: #2563EB;
  border: 1px solid #BFDBFE;
}

.department-name {
  color: #334155;
  font-weight: 600;
  margin: 0 0 2px 0;
}

.role-description {
  color: #64748B;
  margin: 0 0 4px 0;
}

.priority-level {
  color: #94A3B8;
  font-size: 11px;
  margin: 0;
}

.spin-icon {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>

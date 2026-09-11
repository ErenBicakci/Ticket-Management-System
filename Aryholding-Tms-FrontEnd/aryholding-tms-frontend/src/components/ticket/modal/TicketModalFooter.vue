<template>
  <div class="modal-footer">
    <button class="btn btn-secondary" @click="$emit('close')">Kapat</button>

    <!-- Düzenle Butonu -->
    <button
      v-if="canEdit && !isEditing"
      class="btn btn-primary"
      @click="$emit('start-edit')"
    >
      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7"/>
        <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z"/>
      </svg>
      <span>Düzenle</span>
    </button>

    <!-- Kabul Et (Üstlen) Butonu -->
    <button
      v-if="canAccept && !isEditing && !isApprovalStatus && !isAssignedToCurrentUser"
      class="btn btn-accept"
      @click="$emit('accept')"
      :disabled="isAccepting"
    >
      <svg v-if="isAccepting" class="spin-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
        <polyline points="20 6 9 17 4 12"/>
      </svg>
      <span>{{ isAccepting ? 'Üstleniliyor...' : 'Kabul Et' }}</span>
    </button>

    <!-- Başkasına Ata Butonu -->
    <button
      v-if="canAccept && !isEditing && !isApprovalStatus"
      class="btn btn-assign"
      @click="$emit('open-assign')"
      :disabled="isAssigning"
    >
      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M16 21v-2a4 4 0 00-4-4H6a4 4 0 00-4 4v2"/>
        <circle cx="9" cy="7" r="4"/>
        <line x1="19" y1="8" x2="19" y2="14"/>
        <line x1="22" y1="11" x2="16" y2="11"/>
      </svg>
      <span>Başkasına Ata</span>
    </button>

    <!-- Onaya Gönder Butonu -->
    <button
      v-if="canAccept && !isEditing && !isApprovalStatus && isAssignedToCurrentUser"
      class="btn btn-approve"
      @click="$emit('send-approval')"
      :disabled="isApproving"
    >
      <svg v-if="isApproving" class="spin-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M16 4h2a2 2 0 012 2v14a2 2 0 01-2 2H6a2 2 0 01-2-2V6a2 2 0 012-2h2"/>
        <rect x="8" y="2" width="8" height="4" rx="1" ry="1"/>
      </svg>
      <span>{{ isApproving ? 'Gönderiliyor...' : 'Onaya Gönder' }}</span>
    </button>

    <!-- Final Approval Butonları (Yetkili Amir / SuperUser) -->
    <button
      v-if="showFinalApprovalButtons"
      class="btn btn-success"
      @click="$emit('approve')"
      :disabled="isFinalApproving"
    >
      <svg v-if="isFinalApproving" class="spin-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
        <polyline points="20 6 9 17 4 12"/>
      </svg>
      <span>{{ isFinalApproving ? 'Onaylanıyor...' : 'Onayla' }}</span>
    </button>

    <button
      v-if="showFinalApprovalButtons"
      class="btn btn-danger"
      @click="$emit('reject')"
      :disabled="isFinalRejecting"
    >
      <svg v-if="isFinalRejecting" class="spin-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
        <line x1="18" y1="6" x2="6" y2="18"/>
        <line x1="6" y1="6" x2="18" y2="18"/>
      </svg>
      <span>{{ isFinalRejecting ? 'Reddediliyor...' : 'Reddet' }}</span>
    </button>

    <!-- Düzenleme Modu Butonları -->
    <button
      v-if="canEdit && isEditing"
      class="btn btn-success"
      @click="$emit('save-edit')"
      :disabled="isSaving"
    >
      <svg v-if="isSaving" class="spin-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M21 12a9 9 0 11-6.219-8.56"/>
      </svg>
      <span>{{ isSaving ? 'Kaydediliyor...' : 'Kaydet' }}</span>
    </button>

    <button
      v-if="canEdit && isEditing"
      class="btn btn-secondary"
      @click="$emit('cancel-edit')"
    >
      İptal
    </button>
  </div>
</template>

<script>
export default {
  name: 'TicketModalFooter',
  props: {
    canEdit: { type: Boolean, default: false },
    isEditing: { type: Boolean, default: false },
    canAccept: { type: Boolean, default: false },
    isApprovalStatus: { type: Boolean, default: false },
    isAssignedToCurrentUser: { type: Boolean, default: false },
    showFinalApprovalButtons: { type: Boolean, default: false },
    isAccepting: { type: Boolean, default: false },
    isAssigning: { type: Boolean, default: false },
    isApproving: { type: Boolean, default: false },
    isFinalApproving: { type: Boolean, default: false },
    isFinalRejecting: { type: Boolean, default: false },
    isSaving: { type: Boolean, default: false }
  }
}
</script>

<style scoped>
.modal-footer {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  gap: 10px;
  padding: 18px 28px;
  border-top: 1px solid var(--border-color, #E2E8F0);
  background: var(--bg-surface, #FFFFFF);
  flex-wrap: wrap;
  transition: background-color 0.2s ease, border-color 0.2s ease;
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
  border: 1px solid transparent;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-secondary {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  border-color: var(--border-color, #E2E8F0);
}
.btn-secondary:hover:not(:disabled) {
  background: var(--border-subtle, #E2E8F0);
  color: var(--text-main, #0F172A);
}

.btn-primary {
  background: #2563EB;
  color: #FFFFFF;
}
.btn-primary:hover:not(:disabled) {
  background: #1D4ED8;
}

.btn-accept {
  background: #10B981;
  color: #FFFFFF;
}
.btn-accept:hover:not(:disabled) {
  background: #059669;
}

.btn-assign {
  background: #6366F1;
  color: #FFFFFF;
}
.btn-assign:hover:not(:disabled) {
  background: #4F46E5;
}

.btn-approve {
  background: #F59E0B;
  color: #FFFFFF;
}
.btn-approve:hover:not(:disabled) {
  background: #D97706;
}

.btn-success {
  background: #059669;
  color: #FFFFFF;
}
.btn-success:hover:not(:disabled) {
  background: #047857;
}

.btn-danger {
  background: #EF4444;
  color: #FFFFFF;
}
.btn-danger:hover:not(:disabled) {
  background: #DC2626;
}

.spin-icon {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>

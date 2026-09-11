<template>
  <div v-if="isVisible && activeTicket" class="modal-overlay" @click="closeModal">
    <div class="modal-content" @click.stop>
      <!-- 1. Header Component -->
      <TicketModalHeader
        :ticket="activeTicket"
        :is-editing="isEditing"
        v-model:editedTitle="editedTicket.title"
        @close="closeModal"
      />

      <!-- 2. Body -->
      <div class="modal-body">
        <div class="modal-grid">
          <!-- Left Column: Description -->
          <div class="left-col">
            <TicketModalDescription
              :description="activeTicket.description"
              :is-editing="isEditing"
              v-model:editedDescription="editedTicket.description"
            />
          </div>

          <!-- Right Column: Tabs (Yorumlar / Geçmiş) -->
          <div class="right-col">
            <div class="tabs-nav">
              <button
                class="tab-btn"
                :class="{ active: activeTab === 'comments' }"
                @click="activeTab = 'comments'"
              >
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M21 15a2 2 0 01-2 2H7l-4 4V5a2 2 0 012-2h14a2 2 0 012 2z"/>
                </svg>
                <span>Yorumlar</span>
                <span v-if="comments.length > 0" class="tab-badge">{{ comments.length }}</span>
              </button>

              <button
                class="tab-btn"
                :class="{ active: activeTab === 'history' }"
                @click="activeTab = 'history'"
              >
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10"/>
                  <polyline points="12 6 12 12 16 14"/>
                </svg>
                <span>Geçmiş</span>
                <span v-if="ticketHistory.length > 0" class="tab-badge">{{ ticketHistory.length }}</span>
              </button>
            </div>

            <!-- Tab Content -->
            <div class="tab-content-area">
              <TicketCommentsTab
                v-if="activeTab === 'comments'"
                :comments="comments"
                :loading="commentsLoading"
                :error="commentsError"
                :is-submitting="isSubmittingComment"
                @submit-comment="handleSubmitComment"
                @retry="fetchComments"
              />

              <TicketHistoryTab
                v-else-if="activeTab === 'history'"
                :history="ticketHistory"
                :loading="historyLoading"
                :error="historyError"
                @retry="fetchHistory"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 3. Footer Actions -->
      <TicketModalFooter
        :can-edit="canEdit"
        :is-editing="isEditing"
        :can-accept="canAccept"
        :is-approval-status="isApprovalStatus"
        :is-assigned-to-current-user="isAssignedToCurrentUser"
        :show-final-approval-buttons="showFinalApprovalButtons"
        :is-accepting="isAccepting"
        :is-assigning="isAssigning"
        :is-approving="isApproving"
        :is-final-approving="isFinalApproving"
        :is-final-rejecting="isFinalRejecting"
        :is-saving="isSaving"
        @close="closeModal"
        @start-edit="startEditing"
        @cancel-edit="cancelEditing"
        @save-edit="saveChanges"
        @accept="acceptTicket"
        @open-assign="openAssignModal"
        @send-approval="sendForApproval"
        @approve="approveTicket"
        @reject="rejectTicket"
      />
    </div>

    <!-- 4. Extracted Assignment Modal -->
    <TicketUserAssignModal
      :visible="showUserAssignModal"
      :users="departmentUsers"
      :loading="assignLoading"
      :error="assignError"
      @close="closeAssignModal"
      @retry="fetchDepartmentUsers"
      @select-user="assignToUser"
    />
  </div>
</template>

<script>
import { useAuthStore } from '../stores/auth.js'
import { useToastStore } from '../stores/toast.js'
import TicketModalHeader from './ticket/modal/TicketModalHeader.vue'
import TicketModalDescription from './ticket/modal/TicketModalDescription.vue'
import TicketCommentsTab from './ticket/modal/TicketCommentsTab.vue'
import TicketHistoryTab from './ticket/modal/TicketHistoryTab.vue'
import TicketUserAssignModal from './ticket/modal/TicketUserAssignModal.vue'
import TicketModalFooter from './ticket/modal/TicketModalFooter.vue'

export default {
  name: 'TicketDetailModal',
  components: {
    TicketModalHeader,
    TicketModalDescription,
    TicketCommentsTab,
    TicketHistoryTab,
    TicketUserAssignModal,
    TicketModalFooter
  },
  props: {
    isVisible: { type: Boolean, default: false },
    ticket: { type: Object, default: null },
    canEdit: { type: Boolean, default: false },
    canAccept: { type: Boolean, default: false },
    departmentCode: { type: String, default: null }
  },
  data() {
    return {
      localTicket: null,
      activeTab: 'comments',
      isEditing: false,
      isSaving: false,
      isAccepting: false,
      isAssigning: false,
      isApproving: false,
      isFinalApproving: false,
      isFinalRejecting: false,
      editedTicket: {
        title: '',
        description: ''
      },
      // Comments
      comments: [],
      commentsLoading: false,
      commentsError: null,
      isSubmittingComment: false,
      // History
      ticketHistory: [],
      historyLoading: false,
      historyError: null,
      // Assign
      showUserAssignModal: false,
      assignLoading: false,
      assignError: null,
      departmentUsers: []
    }
  },
  computed: {
    authStore() {
      return useAuthStore()
    },
    toastStore() {
      return useToastStore()
    },
    activeTicket() {
      return this.localTicket || this.ticket
    },
    isApprovalStatus() {
      const t = this.activeTicket
      return t && (t.ticketStatusCode === 'WAITING_APPROVAL' || t.ticketStatusCode === 'APPROVED')
    },
    isAssignedToCurrentUser() {
      const t = this.activeTicket
      if (!t || !t.assignedUsername) return false
      return this.authStore.userName === t.assignedUsername
    },
    showFinalApprovalButtons() {
      const t = this.activeTicket
      if (!t || t.ticketStatusCode !== 'WAITING_APPROVAL') return false
      if (this.authStore.isSuperUser) return true
      if (!this.departmentCode) return false

      const deptCodes = this.authStore.departmentCodes
      const priorities = this.authStore.priorities
      const idx = deptCodes.findIndex(c => String(c).toUpperCase() === String(this.departmentCode).toUpperCase())
      if (idx >= 0 && idx < priorities.length) {
        return parseInt(priorities[idx], 10) >= 7
      }
      return false
    }
  },
  watch: {
    isVisible(val) {
      if (val && this.ticket) {
        this.localTicket = { ...this.ticket }
        this.editedTicket.title = this.ticket.title || ''
        this.editedTicket.description = this.ticket.description || ''
        this.isEditing = false
        this.activeTab = 'comments'
        this.fetchComments()
        this.fetchHistory()
      }
    },
    ticket(newVal) {
      if (newVal) {
        this.localTicket = { ...newVal }
        this.editedTicket.title = newVal.title || ''
        this.editedTicket.description = newVal.description || ''
      }
    }
  },
  methods: {
    closeModal() {
      if (this.isEditing) {
        this.isEditing = false
      }
      this.$emit('close')
    },

    startEditing() {
      this.isEditing = true
      this.editedTicket.title = this.activeTicket.title || ''
      this.editedTicket.description = this.activeTicket.description || ''
    },

    cancelEditing() {
      this.isEditing = false
      this.editedTicket.title = this.activeTicket.title || ''
      this.editedTicket.description = this.activeTicket.description || ''
    },

    async saveChanges() {
      if (!this.editedTicket.title.trim()) {
        this.toastStore.warning('Lütfen bir başlık girin.', 'Eksik Bilgi')
        return
      }

      this.isSaving = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          },
          body: JSON.stringify({
            title: this.editedTicket.title.trim(),
            description: this.editedTicket.description.trim()
          })
        })

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)

        this.localTicket.title = this.editedTicket.title.trim()
        this.localTicket.description = this.editedTicket.description.trim()
        this.isEditing = false
        this.toastStore.success('Talep bilgileri başarıyla güncellendi.', 'Başarılı')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message || 'Güncelleme başarısız oldu.', 'Hata')
      } finally {
        this.isSaving = false
      }
    },

    async acceptTicket() {
      this.isAccepting = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}/accept`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)

        this.localTicket.ticketStatusCode = 'ACCEPTED'
        this.localTicket.assignedUsername = this.authStore.userName
        this.toastStore.success('Talep başarıyla üstlenildi.', 'Kabul Edildi')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message || 'Talep üstlenilemedi.', 'Hata')
      } finally {
        this.isAccepting = false
      }
    },

    openAssignModal() {
      this.showUserAssignModal = true
      this.fetchDepartmentUsers()
    },

    closeAssignModal() {
      this.showUserAssignModal = false
    },

    async fetchDepartmentUsers() {
      const deptCode = this.departmentCode || (this.activeTicket && this.activeTicket.departmentCode)
      if (!deptCode) {
        this.departmentUsers = []
        return
      }

      this.assignLoading = true
      this.assignError = null
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/user/department-users?departmentCode=${deptCode}`, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error('Departman kullanıcıları alınamadı.')
        const data = await response.json()
        this.departmentUsers = Array.isArray(data) ? data : []
      } catch (e) {
        this.assignError = e.message
      } finally {
        this.assignLoading = false
      }
    },

    async assignToUser(user) {
      this.isAssigning = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}/assign`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          },
          body: JSON.stringify({
            assignedUsername: user.username
          })
        })

        if (!response.ok) throw new Error('Atama işlemi başarısız.')

        this.localTicket.assignedUsername = user.username
        this.closeAssignModal()
        this.toastStore.success(`Talep @${user.username} kullanıcısına atandı.`, 'Atama Başarılı')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isAssigning = false
      }
    },

    async sendForApproval() {
      this.isApproving = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}/send-for-approval`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error('Onaya gönderme başarısız.')

        this.localTicket.ticketStatusCode = 'WAITING_APPROVAL'
        this.toastStore.success('Talep onaya gönderildi.', 'Başarılı')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isApproving = false
      }
    },

    async approveTicket() {
      this.isFinalApproving = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}/approve`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error('Onay işlemi başarısız.')

        this.localTicket.ticketStatusCode = 'APPROVED'
        this.toastStore.success('Talep başarıyla onaylandı ve tamamlandı.', 'Onaylandı')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isFinalApproving = false
      }
    },

    async rejectTicket() {
      this.isFinalRejecting = true
      try {
        const token = localStorage.getItem('authToken')
        const ticketId = this.activeTicket.ticketId || this.activeTicket.id
        const response = await fetch(`/api/ticket/${ticketId}/reject`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error('Reddetme işlemi başarısız.')

        this.localTicket.ticketStatusCode = 'ACCEPTED'
        this.toastStore.warning('Talep reddedildi ve kabul edilmiş durumuna döndürüldü.', 'Reddedildi')
        this.$emit('ticket-updated', this.localTicket)
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isFinalRejecting = false
      }
    },

    // Comments Fetch & Submit
    async fetchComments() {
      if (!this.activeTicket) return
      const ticketId = this.activeTicket.ticketId || this.activeTicket.id
      this.commentsLoading = true
      this.commentsError = null

      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/ticket/${ticketId}/comments`, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          this.comments = Array.isArray(data) ? data : (data.items || [])
        } else {
          this.comments = []
        }
      } catch (e) {
        this.commentsError = 'Yorumlar yüklenemedi.'
      } finally {
        this.commentsLoading = false
      }
    },

    async handleSubmitComment(commentText) {
      if (!this.activeTicket) return
      const ticketId = this.activeTicket.ticketId || this.activeTicket.id
      this.isSubmittingComment = true

      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/ticket/${ticketId}/comments`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          },
          body: JSON.stringify({
            comment: commentText
          })
        })

        if (!response.ok) throw new Error('Yorum kaydedilemedi.')

        this.toastStore.success('Yorumunuz eklendi.', 'Başarılı')
        this.fetchComments()
        this.fetchHistory()
      } catch (err) {
        this.toastStore.error(err.message, 'Hata')
      } finally {
        this.isSubmittingComment = false
      }
    },

    // History Fetch
    async fetchHistory() {
      if (!this.activeTicket) return
      const ticketId = this.activeTicket.ticketId || this.activeTicket.id
      this.historyLoading = true
      this.historyError = null

      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/ticket/${ticketId}/history`, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          this.ticketHistory = Array.isArray(data) ? data : (data.items || [])
        } else {
          this.ticketHistory = []
        }
      } catch (e) {
        this.historyError = 'Geçmiş kayıtları alınamadı.'
      } finally {
        this.historyLoading = false
      }
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.65);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 20px;
  animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.modal-content {
  background: var(--bg-surface);
  border: 1px solid var(--border-color);
  border-radius: 20px;
  width: 100%;
  max-width: 1080px;
  max-height: 90vh;
  display: flex;
  flex-direction: column;
  box-shadow: 0 25px 60px rgba(0, 0, 0, 0.35);
  overflow: hidden;
  font-family: 'Inter', sans-serif;
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.modal-body {
  padding: 24px 28px;
  overflow-y: auto;
  flex: 1;
}

.modal-grid {
  display: grid;
  grid-template-columns: 55% 45%;
  gap: 24px;
}

.left-col {
  display: flex;
  flex-direction: column;
}

.right-col {
  display: flex;
  flex-direction: column;
  background: var(--bg-surface);
  border: 1px solid var(--border-color);
  border-radius: 14px;
  overflow: hidden;
}

.tabs-nav {
  display: flex;
  border-bottom: 1px solid var(--border-color);
  background: var(--bg-muted);
}

.tab-btn {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px;
  background: transparent;
  border: none;
  border-bottom: 2px solid transparent;
  font-size: 13px;
  font-weight: 600;
  color: var(--text-muted);
  cursor: pointer;
  transition: all 0.2s ease;
}

.tab-btn:hover {
  color: var(--text-main);
  background: var(--bg-hover);
}

.tab-btn.active {
  color: var(--primary-color);
  border-bottom-color: var(--primary-color);
  background: var(--bg-surface);
}

.tab-badge {
  background: var(--primary-light);
  color: var(--primary-color);
  font-size: 11px;
  padding: 1px 7px;
  border-radius: 10px;
  font-weight: 700;
}

.tab-content-area {
  padding: 16px;
  flex: 1;
  overflow-y: auto;
  min-height: 280px;
}

@media (max-width: 900px) {
  .modal-grid {
    grid-template-columns: 1fr;
  }
}
</style>

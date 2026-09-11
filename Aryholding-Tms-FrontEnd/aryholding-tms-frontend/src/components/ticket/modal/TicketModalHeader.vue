<template>
  <div class="modal-header">
    <div class="header-left">
      <h2 v-if="!isEditing">{{ displayTitle }}</h2>
      <div v-else class="header-title-edit">
        <label class="sr-only" for="header-edit-title">Başlık</label>
        <input
          id="header-edit-title"
          :value="editedTitle"
          @input="$emit('update:editedTitle', $event.target.value)"
          type="text"
          class="edit-input header-title-input"
          placeholder="Ticket başlığını girin"
        />
      </div>

      <div class="header-meta">
        <!-- Ticket ID Chip with Click to Copy -->
        <span class="header-chip ticket-id-chip" @click="copyTicketId" title="Kopyalamak için tıklayın">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <rect x="9" y="9" width="13" height="13" rx="2"/>
            <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/>
          </svg>
          #{{ ticket.ticketId }}
        </span>

        <!-- Status Chip -->
        <span class="header-chip status" :class="getStatusClass(ticket.ticketStatusCode)">
          <span class="status-dot"></span>
          {{ getStatusText(ticket.ticketStatusCode) }}
        </span>

        <!-- Severity Badge -->
        <span class="header-chip severity-chip" :class="getSeverityClass(ticket.severityCode)">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <line x1="12" y1="8" x2="12" y2="12"/>
            <line x1="12" y1="16" x2="12.01" y2="16"/>
          </svg>
          {{ getSeverityText(ticket.severityCode) }}
        </span>

        <!-- Category Chip -->
        <span class="header-chip">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"/>
          </svg>
          <strong>{{ getCategoryText(ticket.categoryCode) }}</strong>
        </span>

        <span class="header-chip">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
            <circle cx="12" cy="7" r="4"/>
          </svg>
          {{ ticket.username || 'Bilinmiyor' }}
        </span>

        <span class="header-chip" v-if="ticket.assignedUsername">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="8.5" cy="7" r="4"/>
            <line x1="20" y1="8" x2="20" y2="14"/>
            <line x1="23" y1="11" x2="17" y2="11"/>
          </svg>
          Atanan: <strong>{{ ticket.assignedUsername }}</strong>
        </span>

        <span class="header-chip" v-if="ticket.createdAt">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <polyline points="12 6 12 12 16 14"/>
          </svg>
          {{ formatDate(ticket.createdAt) }}
        </span>
      </div>
    </div>

    <button class="close-btn" @click="$emit('close')" aria-label="Kapat">
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
        <line x1="18" y1="6" x2="6" y2="18"/>
        <line x1="6" y1="6" x2="18" y2="18"/>
      </svg>
    </button>
  </div>
</template>

<script>
import { useToastStore } from '../../../stores/toast.js'

export default {
  name: 'TicketModalHeader',
  props: {
    ticket: {
      type: Object,
      required: true
    },
    isEditing: {
      type: Boolean,
      default: false
    },
    editedTitle: {
      type: String,
      default: ''
    }
  },
  computed: {
    displayTitle() {
      return this.ticket.title || 'Başlık Yok'
    }
  },
  methods: {
    copyTicketId() {
      if (this.ticket && this.ticket.ticketId) {
        navigator.clipboard.writeText(String(this.ticket.ticketId))
        const toastStore = useToastStore()
        toastStore.info(`Talep ID (#${this.ticket.ticketId}) kopyalandı.`, 'Kopyalandı')
      }
    },
    formatDate(dateString) {
      if (!dateString) return '-'
      const date = new Date(dateString)
      return date.toLocaleDateString('tr-TR', {
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    },
    getStatusText(statusCode) {
      const statusMap = {
        'PENDING': 'Beklemede',
        'ACCEPTED': 'Kabul Edildi',
        'WAITING_APPROVAL': 'Onay Bekliyor',
        'APPROVED': 'Tamamlandı'
      }
      return statusMap[statusCode] || statusCode || 'Bilinmiyor'
    },
    getStatusClass(statusCode) {
      const statusClasses = {
        'PENDING': 'status-pending',
        'ACCEPTED': 'status-accepted',
        'WAITING_APPROVAL': 'status-waiting-approval',
        'APPROVED': 'status-approved'
      }
      return statusClasses[statusCode] || 'status-default'
    },
    getSeverityClass(code) {
      const c = String(code || '').toUpperCase()
      return `sev-badge-${c.toLowerCase()}`
    },
    getSeverityText(code) {
      const map = {
        'INFO': 'Bilgi',
        'LOW': 'Düşük',
        'MEDIUM': 'Orta',
        'HIGH': 'Yüksek',
        'CRITICAL': 'Kritik'
      }
      return map[code] || code || 'Belirtilmemiş'
    },
    getCategoryText(categoryCode) {
      if (!categoryCode) return 'Belirtilmemiş'
      const categoryMap = {
        'IT': 'IT',
        'HR': 'İnsan Kaynakları',
        'FINANCE': 'Finans',
        'OPERATIONS': 'Operasyonlar',
        'MAINTENANCE': 'Bakım',
        'TECH-HELP': 'Teknik Yardım',
        'OTHER': 'Diğer'
      }
      return categoryMap[categoryCode] || categoryCode
    }
  }
}
</script>

<style scoped>
.modal-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  padding: 24px 28px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  background: var(--bg-surface, #FFFFFF);
  transition: background-color 0.2s ease, border-color 0.2s ease;
}

.header-left {
  flex: 1;
  min-width: 0;
}

.header-left h2 {
  font-size: 20px;
  font-weight: 700;
  color: var(--text-main, #0F172A);
  margin: 0 0 10px 0;
  line-height: 1.35;
}

.header-title-edit {
  margin-bottom: 10px;
}

.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  border: 0;
}

.header-title-input {
  width: 100%;
  padding: 8px 12px;
  border: 1.5px solid var(--primary-color, #2563EB);
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  color: var(--text-main, #0F172A);
  background: var(--bg-surface, #FFFFFF);
  font-family: inherit;
}

.header-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-items: center;
}

.header-chip {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  border: 1px solid var(--border-color, #E2E8F0);
  font-size: 12px;
  padding: 4px 10px;
  border-radius: 6px;
  display: inline-flex;
  align-items: center;
  gap: 5px;
}

.header-chip strong {
  color: var(--text-main, #0F172A);
  font-weight: 600;
}

.ticket-id-chip {
  background: var(--status-open-bg, #EFF6FF);
  color: var(--status-open-text, #1E40AF);
  border-color: var(--status-open-border, #DBEAFE);
  font-weight: 700;
  cursor: pointer;
}

.ticket-id-chip:hover {
  filter: brightness(0.95);
}

.status-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: currentColor;
}

/* Status Soft Badges */
.status-pending {
  background: var(--status-open-bg);
  color: var(--status-open-text);
  border-color: var(--status-open-border);
  font-weight: 700;
}

.status-accepted {
  background: var(--status-progress-bg);
  color: var(--status-progress-text);
  border-color: var(--status-progress-border);
  font-weight: 700;
}

.status-waiting-approval {
  background: var(--status-progress-bg);
  color: var(--status-progress-text);
  border-color: var(--status-progress-border);
  font-weight: 700;
}

.status-approved {
  background: var(--status-resolved-bg);
  color: var(--status-resolved-text);
  border-color: var(--status-resolved-border);
  font-weight: 700;
}

/* Severity Badges */
.sev-badge-info { background: var(--severity-info-bg); color: var(--severity-info-text); border-color: var(--severity-info-border); font-weight: 600; }
.sev-badge-low { background: var(--severity-low-bg); color: var(--severity-low-text); border-color: var(--severity-low-border); font-weight: 600; }
.sev-badge-medium { background: var(--severity-medium-bg); color: var(--severity-medium-text); border-color: var(--severity-medium-border); font-weight: 600; }
.sev-badge-high { background: var(--severity-high-bg); color: var(--severity-high-text); border-color: var(--severity-high-border); font-weight: 600; }
.sev-badge-critical { background: var(--severity-critical-bg); color: var(--severity-critical-text); border-color: var(--severity-critical-border); font-weight: 700; }

.close-btn {
  background: var(--bg-muted, #F1F5F9);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 8px;
  padding: 8px;
  cursor: pointer;
  color: var(--text-secondary, #64748B);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.close-btn:hover {
  background: var(--border-subtle, #E2E8F0);
  color: var(--text-main, #0F172A);
}
</style>

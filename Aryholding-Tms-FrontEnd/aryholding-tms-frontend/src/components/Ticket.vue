<template>
  <div
    class="ticket-card"
    :class="[getPriorityClass(), { 'is-dragging': isDragging }]"
    draggable="true"
    @dragstart="onDragStart"
    @dragend="onDragEnd"
    @click="handleTicketClick"
  >
    <!-- Thin colored left accent -->
    <div class="severity-accent" :class="getSeverityClass()"></div>

    <div class="ticket-content">
      <!-- Header: title + severity chip -->
      <div class="ticket-header">
        <h3 class="ticket-title" :title="ticket.title">{{ ticket.title }}</h3>
        <span
          v-if="ticket.severityCode"
          class="severity-chip"
          :class="getSeverityBadgeClass()"
        >
          <span class="severity-dot"></span>
          {{ getSeverityText(ticket.severityCode) }}
        </span>
      </div>

      <!-- Meta: id + date -->
      <div class="ticket-meta">
        <span class="ticket-id">#{{ ticket.ticketId }}</span>
        <span class="ticket-date">{{ formatDate(ticket.createdAt) }}</span>
      </div>

      <!-- Details grid -->
      <div class="ticket-details">
        <div class="detail-row">
          <div class="detail-item">
            <span class="detail-label">Kıdem</span>
            <span class="detail-value">{{ getPriorityText(ticket.priorityLevel) }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Kategori</span>
            <span class="detail-value">{{ getCategoryText(ticket.categoryCode) }}</span>
          </div>
        </div>
      </div>

      <!-- Assigned / Creator Avatar Footer -->
      <div class="ticket-assign-row" v-if="ticket.assignedUsername || ticket.username">
        <div class="user-chip" :title="'Atanan: ' + (ticket.assignedUsername || 'Atanmamış')">
          <div class="user-avatar-mini">
            {{ (ticket.assignedUsername || ticket.username || '?').charAt(0).toUpperCase() }}
          </div>
          <span class="user-name-mini">{{ ticket.assignedUsername || ticket.username }}</span>
        </div>

        <button
          v-if="canAssign"
          class="assign-quick-btn"
          @click.stop="assignTicket"
          title="Talebi Ata"
        >
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="8.5" cy="7" r="4"/>
            <line x1="20" y1="8" x2="20" y2="14"/>
            <line x1="23" y1="11" x2="17" y2="11"/>
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TicketCard',
  props: {
    ticket: { type: Object, required: true },
    canEdit: { type: Boolean, default: false },
    canAssign: { type: Boolean, default: false }
  },
  data() {
    return {
      isDragging: false
    }
  },
  methods: {
    onDragStart(event) {
      this.isDragging = true
      event.dataTransfer.effectAllowed = 'move'
      event.dataTransfer.setData('text/plain', JSON.stringify({
        ticketId: this.ticket.ticketId,
        currentStatusCode: this.ticket.ticketStatusCode
      }))
    },
    onDragEnd() {
      this.isDragging = false
    },
    formatDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleDateString('tr-TR', {
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    },
    getPriorityText(priorityLevel) {
      if (!priorityLevel) return 'Belirtilmemiş'
      return priorityLevel
    },
    getPriorityClass() {
      if (!this.ticket.priorityLevel) return 'priority-default'
      return `priority-${this.ticket.priorityLevel}`
    },
    getSeverityClass() {
      if (!this.ticket.severityCode) return 'severity-null'
      return `severity-${this.ticket.severityCode}`
    },
    getSeverityBadgeClass() {
      const code = (this.ticket.severityCode || '').toLowerCase()
      return `badge-${code}`
    },
    getSeverityText(severityCode) {
      if (!severityCode) return 'Belirtilmemiş'
      const severityMap = {
        'INFO': 'Bilgi',
        'LOW': 'Düşük',
        'MEDIUM': 'Orta',
        'HIGH': 'Yüksek',
        'CRITICAL': 'Kritik'
      }
      return severityMap[severityCode] || severityCode
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
    },
    handleTicketClick() {
      this.$emit('edit-ticket', this.ticket.ticketId)
    },
    assignTicket() {
      this.$emit('assign-ticket', this.ticket.ticketId)
    }
  }
}
</script>

<style scoped>
.ticket-card {
  background: var(--bg-surface, #FFFFFF);
  border-radius: 12px;
  box-shadow: var(--shadow-sm);
  transition: transform 0.2s cubic-bezier(0.16, 1, 0.3, 1), box-shadow 0.2s ease, border-color 0.2s ease;
  cursor: pointer;
  display: flex;
  position: relative;
  overflow: hidden;
  border: 1px solid var(--border-color, #E2E8F0);
  font-family: 'Inter', sans-serif;
  user-select: none;
}

.ticket-card:hover {
  transform: translateY(-3px);
  box-shadow: var(--shadow-card-hover);
  border-color: var(--primary-border, #BFDBFE);
}

.ticket-card.is-dragging {
  opacity: 0.45;
  transform: scale(0.97);
  box-shadow: none;
}

/* Severity accent bar on left */
.severity-accent {
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 5px;
  border-radius: 12px 0 0 12px;
  z-index: 1;
  flex-shrink: 0;
}

.severity-null     { background-color: var(--text-muted); }
.severity-INFO     { background-color: #0284C7; }
.severity-LOW      { background-color: #16A34A; }
.severity-MEDIUM   { background-color: #CA8A04; }
.severity-HIGH     { background-color: #EA580C; }
.severity-CRITICAL { background-color: #DC2626; }

.ticket-content {
  flex: 1;
  padding: 14px 16px;
  margin-left: 5px;
  display: flex;
  flex-direction: column;
  min-width: 0;
}

.ticket-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 8px;
  margin-bottom: 6px;
}

.ticket-title {
  color: var(--text-main, #1E293B);
  font-size: 13px;
  font-weight: 700;
  margin: 0;
  line-height: 1.4;
  flex: 1;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
}

/* Soft-tinted severity badges */
.severity-chip {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 2px 8px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 700;
  border: 1px solid transparent;
  white-space: nowrap;
  flex-shrink: 0;
  letter-spacing: 0.2px;
}

.severity-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
  flex-shrink: 0;
}

.badge-info {
  background: var(--severity-info-bg);
  color: var(--severity-info-text);
  border-color: var(--severity-info-border);
}

.badge-low {
  background: var(--severity-low-bg);
  color: var(--severity-low-text);
  border-color: var(--severity-low-border);
}

.badge-medium {
  background: var(--severity-medium-bg);
  color: var(--severity-medium-text);
  border-color: var(--severity-medium-border);
}

.badge-high {
  background: var(--severity-high-bg);
  color: var(--severity-high-text);
  border-color: var(--severity-high-border);
}

.badge-critical {
  background: var(--severity-critical-bg);
  color: var(--severity-critical-text);
  border-color: var(--severity-critical-border);
}

.ticket-meta {
  display: flex;
  gap: 8px;
  align-items: center;
  margin-bottom: 10px;
}

.ticket-id {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-muted, #64748B);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 2px 6px;
  border-radius: 5px;
  font-size: 10px;
  font-weight: 700;
}

.ticket-date {
  color: var(--text-muted, #94A3B8);
  font-size: 11px;
}

.ticket-details {
  background: var(--bg-muted, #F8FAFC);
  border-radius: 8px;
  padding: 8px 10px;
  border: 1px solid var(--border-color, #F1F5F9);
  margin-bottom: 10px;
}

.detail-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px;
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.detail-label {
  color: var(--text-muted, #94A3B8);
  font-size: 10px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.detail-value {
  color: var(--text-secondary, #374151);
  font-size: 12px;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* Footer row */
.ticket-assign-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-top: 8px;
  border-top: 1px solid var(--border-color, #F1F5F9);
  gap: 8px;
}

.user-chip {
  display: flex;
  align-items: center;
  gap: 6px;
  min-width: 0;
}

.user-avatar-mini {
  width: 22px;
  height: 22px;
  border-radius: 50%;
  background: linear-gradient(135deg, #3B82F6, #60A5FA);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 10px;
  font-weight: 700;
  flex-shrink: 0;
}

.user-name-mini {
  font-size: 11px;
  color: var(--text-muted, #64748B);
  font-weight: 500;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.assign-quick-btn {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  color: var(--text-muted, #64748B);
  padding: 4px 6px;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.15s ease;
}

.assign-quick-btn:hover {
  background: var(--primary-light, #EFF6FF);
  color: var(--primary-color, #2563EB);
  border-color: var(--primary-border, #BFDBFE);
}
</style>

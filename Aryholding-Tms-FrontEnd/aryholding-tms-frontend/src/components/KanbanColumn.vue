<template>
  <div
    :class="[
      'kanban-column',
      statusClass,
      { 'is-drag-over': isDragOver }
    ]"
    @dragover.prevent="onDragOver"
    @dragleave="onDragLeave"
    @drop.prevent="onDrop"
  >
    <!-- Column Header -->
    <div :class="['column-header', statusClass]">
      <div class="column-header-left">
        <div class="column-status-dot"></div>
        <h3>{{ title }}</h3>
      </div>
      <span class="ticket-count">{{ tickets.length }}</span>
    </div>

    <!-- Drop Glow Indicator Line -->
    <div v-if="isDragOver" class="drop-glow-line">
      <span>Bu duruma bırakın</span>
    </div>

    <!-- Column Content -->
    <div :class="['column-content', statusClass]">
      <!-- Pagination -->
      <div class="pagination top-pagination">
        <button 
          @click="changePage(page - 1)" 
          :disabled="page <= 1"
          class="page-btn"
          title="Önceki Sayfa"
        >
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="15 18 9 12 15 6"/>
          </svg>
        </button>
        <span class="page-info">Sayfa {{ page }}</span>
        <button 
          @click="changePage(page + 1)" 
          :disabled="tickets.length < pageSize"
          class="page-btn"
          title="Sonraki Sayfa"
        >
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="9 18 15 12 9 6"/>
          </svg>
        </button>
      </div>
      
      <!-- Loading State: Skeleton Shimmer -->
      <div v-if="loading" class="column-loading">
        <SkeletonLoader type="card" :count="2" />
      </div>
      
      <!-- Empty State -->
      <div v-else-if="tickets.length === 0" class="empty-column">
        <div class="empty-icon-wrap">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round">
            <rect x="3" y="4" width="18" height="18" rx="2"/>
            <path d="M8 9h8M8 13h6M8 17h4"/>
          </svg>
        </div>
        <p>Bu sütunda henüz talep yok</p>
      </div>
      
      <!-- Ticket Cards List -->
      <div v-else class="tickets-container">
        <TicketCard 
          v-for="ticket in tickets" 
          :key="ticket.ticketId"
          :ticket="ticket"
          :can-edit="canEdit"
          :can-assign="canAssign"
          @edit-ticket="$emit('edit-ticket', ticket.ticketId)"
          @assign-ticket="$emit('assign-ticket', ticket.ticketId)"
        />
      </div>
    </div>
  </div>
</template>

<script>
import TicketCard from './Ticket.vue'
import SkeletonLoader from './common/SkeletonLoader.vue'

export default {
  name: 'KanbanColumn',
  components: {
    TicketCard,
    SkeletonLoader
  },
  props: {
    title: { type: String, required: true },
    statusCode: { type: String, required: true },
    tickets: { type: Array, default: () => [] },
    loading: { type: Boolean, default: false },
    page: { type: Number, default: 1 },
    pageSize: { type: Number, default: 10 },
    totalCount: { type: Number, default: 0 },
    canEdit: { type: Boolean, default: true },
    canAssign: { type: Boolean, default: false }
  },
  data() {
    return {
      isDragOver: false
    }
  },
  computed: {
    statusClass() {
      const statusMap = {
        'PENDING': 'pending',
        'ACCEPTED': 'accepted',
        'WAITING_APPROVAL': 'waiting-approval',
        'APPROVED': 'approved'
      }
      return statusMap[this.statusCode] || 'pending'
    }
  },
  methods: {
    changePage(newPage) {
      if (newPage < 1) return
      this.$emit('page-change', this.statusCode, newPage)
    },
    onDragOver() {
      this.isDragOver = true
    },
    onDragLeave() {
      this.isDragOver = false
    },
    onDrop(event) {
      this.isDragOver = false
      try {
        const rawData = event.dataTransfer.getData('text/plain')
        if (!rawData) return
        const data = JSON.parse(rawData)
        if (data.ticketId) {
          this.$emit('ticket-dropped', {
            ticketId: data.ticketId,
            currentStatusCode: data.currentStatusCode,
            newStatusCode: this.statusCode
          })
        }
      } catch (e) {
        console.error('Drag drop parse error:', e)
      }
    }
  }
}
</script>

<style scoped>
.kanban-column {
  background: var(--bg-surface, #FFFFFF);
  border-radius: 16px;
  border: 1px solid var(--border-color, #E2E8F0);
  min-height: 600px;
  height: 100%;
  display: flex;
  flex-direction: column;
  min-width: 0;
  flex: 1;
  box-shadow: var(--shadow-sm);
  transition: all 0.25s ease;
  position: relative;
  overflow: hidden;
  font-family: 'Inter', sans-serif;
}

.kanban-column:hover {
  box-shadow: var(--shadow-md);
}

.kanban-column.is-drag-over {
  border-color: var(--primary-color, #2563EB);
  box-shadow: 0 0 0 2px var(--primary-border, rgba(37, 99, 235, 0.3));
  background: var(--primary-light, #EFF6FF);
}

.drop-glow-line {
  background: var(--primary-color, #2563EB);
  color: #FFFFFF;
  font-size: 11px;
  font-weight: 700;
  text-align: center;
  padding: 4px;
  letter-spacing: 0.5px;
  animation: pulse 1.5s infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.7; }
}

/* Column Header */
.column-header {
  padding: 16px 18px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: white;
  flex-shrink: 0;
}

.column-header-left {
  display: flex;
  align-items: center;
  gap: 10px;
}

.column-status-dot {
  width: 9px;
  height: 9px;
  border-radius: 50%;
  background: rgba(255,255,255,0.8);
  flex-shrink: 0;
}

.column-header h3 {
  margin: 0;
  font-size: 14px;
  font-weight: 700;
  color: #FFFFFF;
  letter-spacing: 0.2px;
}

.ticket-count {
  background: rgba(255,255,255,0.22);
  color: #FFFFFF;
  border-radius: 20px;
  padding: 2px 9px;
  font-size: 11px;
  font-weight: 700;
}

.column-header.pending { background: linear-gradient(135deg, #D97706, #F59E0B); }
.column-header.accepted { background: linear-gradient(135deg, #059669, #10B981); }
.column-header.waiting-approval { background: linear-gradient(135deg, #0284C7, #38BDF8); }
.column-header.approved { background: linear-gradient(135deg, #7C3AED, #A78BFA); }

/* Column Content */
.column-content {
  padding: 14px;
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 10px;
  min-height: 0;
  background: var(--bg-muted, #F8FAFC);
  border-radius: 0 0 16px 16px;
  transition: background-color 0.25s ease;
}

/* Loading */
.column-loading {
  padding: 10px 0;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

/* Empty State */
.empty-column {
  text-align: center;
  padding: 40px 16px;
  color: var(--text-muted, #94A3B8);
  font-size: 13px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.empty-icon-wrap {
  color: var(--text-muted, #CBD5E1);
}

.empty-column p {
  margin: 0;
  font-style: italic;
}

.tickets-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

/* Pagination */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
}

.top-pagination {
  padding-bottom: 10px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  margin-bottom: 2px;
}

.page-btn {
  background: var(--bg-surface, #FFFFFF);
  color: var(--text-secondary, #475569);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 6px 8px;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.page-btn:hover:not(:disabled) {
  background: var(--primary-light, #EFF6FF);
  border-color: var(--primary-color, #2563EB);
  color: var(--primary-color, #2563EB);
}

.page-btn:disabled {
  opacity: 0.35;
  cursor: not-allowed;
}

.page-info {
  color: var(--text-muted, #64748B);
  font-weight: 600;
  font-size: 11px;
  min-width: 50px;
  text-align: center;
}
</style>

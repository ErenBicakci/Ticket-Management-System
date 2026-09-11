<template>
  <div class="history-tab-wrap">
    <!-- Loading Skeleton -->
    <div v-if="loading" class="history-skeleton">
      <SkeletonLoader type="text" :count="5" height="48px" />
    </div>

    <!-- Error -->
    <div v-else-if="error" class="state-box error-box">
      <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <circle cx="12" cy="12" r="10"/>
        <line x1="15" y1="9" x2="9" y2="15"/>
        <line x1="9" y1="9" x2="15" y2="15"/>
      </svg>
      <p>{{ error }}</p>
      <button @click="$emit('retry')" class="retry-btn">Tekrar Dene</button>
    </div>

    <!-- Empty -->
    <div v-else-if="history.length === 0" class="empty-history">
      <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
        <circle cx="12" cy="12" r="10"/>
        <polyline points="12 6 12 12 16 14"/>
      </svg>
      <p>Bu talep için henüz bir geçmiş kaydı bulunmuyor.</p>
    </div>

    <!-- Timeline List -->
    <div v-else class="timeline-list">
      <div
        v-for="(item, idx) in history"
        :key="item.id || idx"
        class="timeline-item"
      >
        <div class="tl-left">
          <div class="tl-dot" :class="[`dot-${(item.eventCode || '').toLowerCase()}`]">
            <!-- CREATED -->
            <svg v-if="item.eventCode === 'CREATED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M12 5v14M5 12h14"/>
            </svg>
            <!-- UPDATED -->
            <svg v-else-if="item.eventCode === 'UPDATED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
              <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
            </svg>
            <!-- ACCEPTED -->
            <svg v-else-if="item.eventCode === 'ACCEPTED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="20 6 9 17 4 12"/>
            </svg>
            <!-- ASSIGNED -->
            <svg v-else-if="item.eventCode === 'ASSIGNED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
              <circle cx="8.5" cy="7" r="4"/>
              <line x1="20" y1="8" x2="20" y2="14"/>
              <line x1="23" y1="11" x2="17" y2="11"/>
            </svg>
            <!-- WAITING_APPROVAL -->
            <svg v-else-if="item.eventCode === 'WAITING_APPROVAL'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="10"/>
              <polyline points="12 6 12 12 16 14"/>
            </svg>
            <!-- APPROVED -->
            <svg v-else-if="item.eventCode === 'APPROVED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/>
              <polyline points="22 4 12 14.01 9 11.01"/>
            </svg>
            <!-- REJECTED -->
            <svg v-else-if="item.eventCode === 'REJECTED'" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="10"/>
              <line x1="15" y1="9" x2="9" y2="15"/>
              <line x1="9" y1="9" x2="15" y2="15"/>
            </svg>
            <!-- DEFAULT -->
            <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="4"/>
            </svg>
          </div>
          <div v-if="idx !== history.length - 1" class="tl-line"></div>
        </div>

        <div class="tl-body">
          <span class="tl-label" :class="[`label-${(item.eventCode || '').toLowerCase()}`]">
            {{ getEventLabel(item.eventCode) }}
          </span>
          <p v-if="item.eventMessage" class="tl-message">{{ item.eventMessage }}</p>
          <span v-if="item.createdAt" class="tl-time">{{ formatDate(item.createdAt) }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import SkeletonLoader from '../../common/SkeletonLoader.vue'

export default {
  name: 'TicketHistoryTab',
  components: {
    SkeletonLoader
  },
  props: {
    history: {
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
  methods: {
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
    getEventLabel(eventCode) {
      const code = String(eventCode || '').toUpperCase()
      const map = {
        'CREATED': 'Oluşturuldu',
        'UPDATED': 'Güncellendi',
        'ACCEPTED': 'Kabul Edildi',
        'ASSIGNED': 'Atandı',
        'WAITING_APPROVAL': 'Onaya Gönderildi',
        'APPROVED': 'Onaylandı',
        'REJECTED': 'Reddedildi'
      }
      return map[code] || code || 'İşlem'
    }
  }
}
</script>

<style scoped>
.history-tab-wrap {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.history-skeleton {
  padding: 16px 8px;
}

.state-box,
.empty-history {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 10px;
  padding: 40px 20px;
  color: var(--text-muted, #94A3B8);
  font-size: 13px;
  text-align: center;
}

.timeline-list {
  display: flex;
  flex-direction: column;
  max-height: 440px;
  overflow-y: auto;
  padding: 8px 4px;
}

.timeline-item {
  display: flex;
  gap: 14px;
  position: relative;
}

.tl-left {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.tl-dot {
  width: 26px;
  height: 26px;
  border-radius: 50%;
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  z-index: 2;
  background: var(--text-muted, #64748B);
}

.dot-created { background: #2563EB; }
.dot-updated { background: #3B82F6; }
.dot-accepted { background: #10B981; }
.dot-assigned { background: #8B5CF6; }
.dot-waiting_approval { background: #F59E0B; }
.dot-approved { background: #059669; }
.dot-rejected { background: #EF4444; }

.tl-line {
  width: 2px;
  flex: 1;
  background: var(--border-color, #E2E8F0);
  margin: 4px 0;
}

.tl-body {
  flex: 1;
  padding-bottom: 20px;
}

.tl-label {
  display: inline-block;
  padding: 3px 8px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
  margin-bottom: 4px;
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  border: 1px solid var(--border-color, #E2E8F0);
}

.label-created { background: rgba(37, 99, 235, 0.12); color: #2563EB; border-color: rgba(37, 99, 235, 0.3); }
.label-updated { background: rgba(59, 130, 246, 0.12); color: #3B82F6; border-color: rgba(59, 130, 246, 0.3); }
.label-accepted { background: rgba(16, 185, 129, 0.12); color: #10B981; border-color: rgba(16, 185, 129, 0.3); }
.label-assigned { background: rgba(139, 92, 246, 0.12); color: #8B5CF6; border-color: rgba(139, 92, 246, 0.3); }
.label-waiting_approval { background: rgba(245, 158, 11, 0.12); color: #D97706; border-color: rgba(245, 158, 11, 0.3); }
.label-approved { background: rgba(5, 150, 105, 0.12); color: #059669; border-color: rgba(5, 150, 105, 0.3); }
.label-rejected { background: rgba(239, 68, 68, 0.12); color: #DC2626; border-color: rgba(239, 68, 68, 0.3); }

.tl-message {
  font-size: 13px;
  color: var(--text-main, #334155);
  margin: 2px 0 4px 0;
  line-height: 1.45;
}

.tl-time {
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
  display: block;
}

.retry-btn {
  background: var(--bg-surface);
  border: 1px solid var(--border-color);
  color: var(--text-main);
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 12px;
  cursor: pointer;
  margin-top: 8px;
}
</style>

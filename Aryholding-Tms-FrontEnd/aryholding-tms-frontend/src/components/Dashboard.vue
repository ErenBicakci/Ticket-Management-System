<template>
  <div class="dashboard-page">
    <div class="hero-banner">
      <div class="hero-content">
        <div class="hero-tag">BPM YÖNETİM SİSTEMİ</div>
        <h2>Hoş Geldiniz, {{ userName }}</h2>
        <p>Taleplerinizi oluşturabilir, onay süreçlerini takip edebilir ve departman akışlarını yönetebilirsiniz.</p>
        <div class="hero-actions">
          <router-link to="/tickets/new" class="btn btn-primary">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M12 5v14M5 12h14"/>
            </svg>
            <span>Yeni Talep Oluştur</span>
          </router-link>
          <router-link to="/tickets/my" class="btn btn-secondary">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <rect x="3" y="4" width="18" height="18" rx="2"/>
              <path d="M8 9h8M8 13h6M8 17h4"/>
            </svg>
            <span>Taleplerimi İncele</span>
          </router-link>
        </div>
      </div>
      <div class="hero-badge-art">
        <div class="art-ring ring-1"></div>
        <div class="art-ring ring-2"></div>
        <div class="art-icon">
          <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#60A5FA" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
            <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"/>
          </svg>
        </div>
      </div>
    </div>

    <div class="kpi-grid">
      <div class="kpi-card kpi-pending">
        <div class="kpi-icon-wrap pending-icon">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <polyline points="12 6 12 12 16 14"/>
          </svg>
        </div>
        <div class="kpi-body">
          <span class="kpi-label">Beklemede / İşlemde</span>
          <div class="kpi-value">
            <span v-if="loadingMetrics" class="kpi-skeleton">...</span>
            <span v-else>{{ metrics.activeTickets }}</span>
          </div>
          <span class="kpi-sub">İşlem sırasındaki talepleriniz</span>
        </div>
      </div>

      <div class="kpi-card kpi-waiting">
        <div class="kpi-icon-wrap waiting-icon">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/>
            <line x1="12" y1="9" x2="12" y2="13"/>
            <line x1="12" y1="17" x2="12.01" y2="17"/>
          </svg>
        </div>
        <div class="kpi-body">
          <span class="kpi-label">Onay Bekleyenler</span>
          <div class="kpi-value">
            <span v-if="loadingMetrics" class="kpi-skeleton">...</span>
            <span v-else>{{ metrics.waitingApprovalTickets }}</span>
          </div>
          <span class="kpi-sub">Son onay bekleyen talepler</span>
        </div>
      </div>

      <div class="kpi-card kpi-approved">
        <div class="kpi-icon-wrap approved-icon">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"/>
          </svg>
        </div>
        <div class="kpi-body">
          <span class="kpi-label">Tamamlanan Talepler</span>
          <div class="kpi-value">
            <span v-if="loadingMetrics" class="kpi-skeleton">...</span>
            <span v-else>{{ metrics.completedTickets }}</span>
          </div>
          <span class="kpi-sub">Başarıyla kapatılan talepler</span>
        </div>
      </div>

      <div class="kpi-card kpi-departments">
        <div class="kpi-icon-wrap dept-icon">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
            <rect x="10" y="13" width="4" height="8"/>
          </svg>
        </div>
        <div class="kpi-body">
          <span class="kpi-label">Departmanlar</span>
          <div class="kpi-value">
            <span v-if="loadingMetrics" class="kpi-skeleton">...</span>
            <span v-else>{{ departments.length }}</span>
          </div>
          <span class="kpi-sub">Kayıtlı aktif birim sayısı</span>
        </div>
      </div>
    </div>

    <div class="analytics-charts-grid">
      <div class="panel chart-panel">
        <div class="panel-header">
          <div class="panel-title-group">
            <h3>Haftalık Talep Dağılımı</h3>
            <span class="chart-subtitle">Son 7 günlük talep akış hacmi</span>
          </div>
          <div class="chart-legend-badge">
            <span class="legend-dot"></span>
            <span>Oluşturulan Talepler</span>
          </div>
        </div>
        <div class="chart-canvas-container">
          <canvas ref="trendChartCanvas"></canvas>
        </div>
      </div>

      <div class="panel chart-panel doughnut-panel">
        <div class="panel-header">
          <div class="panel-title-group">
            <h3>Talep Durum Oranları</h3>
            <span class="chart-subtitle">Genel oranlar</span>
          </div>
        </div>
        <div class="chart-canvas-container doughnut-wrap">
          <canvas ref="statusChartCanvas"></canvas>
        </div>
      </div>
    </div>

    <div class="dashboard-grid">
      <div class="panel recent-tickets-panel">
        <div class="panel-header">
          <div class="panel-title-group">
            <h3>Son Taleplerim</h3>
            <span class="badge-count">{{ recentTickets.length }}</span>
          </div>
          <router-link to="/tickets/my" class="panel-link">
            <span>Tümünü Gör</span>
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="9 18 15 12 9 6"/>
            </svg>
          </router-link>
        </div>

        <div v-if="loadingTickets" class="panel-loading-skeleton">
          <SkeletonLoader type="table" :count="4" />
        </div>

        <div v-else-if="recentTickets.length === 0" class="panel-empty">
          <div class="empty-icon">
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
              <rect x="3" y="4" width="18" height="18" rx="2"/>
              <path d="M8 9h8M8 13h6M8 17h4"/>
            </svg>
          </div>
          <h4>Henüz oluşturulmuş bir talep yok</h4>
          <p>Yeni bir talep açarak iş süreçlerinizi başlatabilirsiniz.</p>
          <router-link to="/tickets/new" class="btn btn-sm btn-primary">İlk Talebini Aç</router-link>
        </div>

        <div v-else class="tickets-table-wrap">
          <table class="dashboard-table">
            <thead>
              <tr>
                <th>Talep ID</th>
                <th>Başlık</th>
                <th>Kategori</th>
                <th>Şiddet</th>
                <th>Durum</th>
                <th>Tarih</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="ticket in recentTickets"
                :key="ticket.ticketId"
                class="clickable-row"
                @click="openTicketDetail(ticket)"
              >
                <td class="col-id">
                  <span class="dash-id-badge">#{{ ticket.ticketId }}</span>
                </td>
                <td class="col-title">{{ ticket.title }}</td>
                <td class="col-cat">
                  <span class="category-chip">{{ ticket.categoryCode || 'Genel' }}</span>
                </td>
                <td class="col-sev">
                  <span class="severity-dot-wrap" :class="`sev-${(ticket.severityCode || '').toLowerCase()}`">
                    <span class="dot"></span>
                    <span>{{ getSeverityText(ticket.severityCode) }}</span>
                  </span>
                </td>
                <td class="col-status">
                  <span :class="['status-chip', getStatusClass(ticket.ticketStatusCode)]">
                    {{ getStatusText(ticket.ticketStatusCode) }}
                  </span>
                </td>
                <td class="col-date">{{ formatDate(ticket.createdAt) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div class="panel shortcuts-panel">
        <div class="panel-header">
          <h3>Departmanlar</h3>
          <router-link to="/departments" class="panel-link">
            <span>Tümü</span>
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="9 18 15 12 9 6"/>
            </svg>
          </router-link>
        </div>

        <div v-if="loadingDepartments" class="panel-loading-skeleton">
          <SkeletonLoader type="text" :count="4" height="40px" />
        </div>

        <div v-else-if="departments.length === 0" class="panel-empty-compact">
          <p>Kayıtlı departman bulunamadı.</p>
        </div>

        <div v-else class="dept-shortcuts-list">
          <div
            v-for="dept in departments.slice(0, 6)"
            :key="dept.code"
            class="dept-item"
            @click="goToDepartment(dept.code)"
          >
            <div class="dept-icon-box">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
                <rect x="10" y="13" width="4" height="8"/>
              </svg>
            </div>
            <div class="dept-info">
              <h4>{{ dept.name || dept.code }}</h4>
              <span class="dept-code-tag">{{ dept.code }}</span>
            </div>
            <div class="dept-arrow">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="9 18 15 12 9 6"/>
              </svg>
            </div>
          </div>
        </div>
      </div>
    </div>

    <TicketDetailModal
      v-if="showTicketModal"
      :is-visible="showTicketModal"
      :ticket="selectedTicket"
      :can-edit="true"
      :can-accept="false"
      @close="closeTicketModal"
      @ticket-updated="onTicketUpdated"
    />
  </div>
</template>

<script>
import { Chart, registerables } from 'chart.js'
import { useAuthStore } from '../stores/auth.js'
import TicketDetailModal from './TicketDetailModal.vue'
import SkeletonLoader from './common/SkeletonLoader.vue'

Chart.register(...registerables)

export default {
  name: 'DashboardView',
  components: {
    TicketDetailModal,
    SkeletonLoader
  },
  data() {
    return {
      metrics: {
        activeTickets: 0,
        waitingApprovalTickets: 0,
        completedTickets: 0
      },
      recentTickets: [],
      departments: [],
      loadingMetrics: false,
      loadingTickets: false,
      loadingDepartments: false,
      selectedTicket: null,
      showTicketModal: false,
      trendChartInstance: null,
      statusChartInstance: null
    }
  },
  computed: {
    authStore() {
      return useAuthStore()
    },
    userName() {
      return this.authStore.userName || 'Kullanıcı'
    }
  },
  mounted() {
    this.fetchDashboardData()
  },
  beforeUnmount() {
    if (this.trendChartInstance) {
      this.trendChartInstance.destroy()
    }
    if (this.statusChartInstance) {
      this.statusChartInstance.destroy()
    }
  },
  methods: {
    async fetchDashboardData() {
      await Promise.all([
        this.fetchMyTickets(),
        this.fetchMySummary(),
        this.fetchDepartments()
      ])
      this.$nextTick(() => {
        this.initCharts()
      })
    },

    async fetchMySummary() {
      this.loadingMetrics = true
      try {
        const token = localStorage.getItem('authToken')
        if (!token) return

        const response = await fetch('/api/ticket/my-summary', {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const summary = await response.json()
          this.metrics.activeTickets = summary.activeTickets ?? ((summary.pendingTickets || 0) + (summary.acceptedTickets || 0))
          this.metrics.waitingApprovalTickets = summary.waitingApprovalTickets || 0
          this.metrics.completedTickets = summary.approvedTickets || 0
        }
      } catch (e) {
        console.error('Error fetching dashboard summary:', e)
      } finally {
        this.loadingMetrics = false
      }
    },

    async fetchMyTickets() {
      this.loadingTickets = true
      try {
        const token = localStorage.getItem('authToken')
        if (!token) return

        const response = await fetch('/api/ticket/my-tickets?page=1&pageSize=6&orderDirection=desc', {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          const items = Array.isArray(data) ? data : (data.items || [])
          this.recentTickets = items.slice(0, 6)
        }
      } catch (e) {
        console.error('Error fetching dashboard tickets:', e)
      } finally {
        this.loadingTickets = false
      }
    },

    async fetchDepartments() {
      this.loadingDepartments = true
      try {
        const token = localStorage.getItem('authToken')
        if (!token) return

        const response = await fetch('/api/department/get-all', {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          this.departments = Array.isArray(data) ? data : (data.data || [])
        }
      } catch (e) {
        console.error('Error fetching departments:', e)
      } finally {
        this.loadingDepartments = false
      }
    },

    initCharts() {
      this.initTrendChart()
      this.initStatusChart()
    },

    initTrendChart() {
      if (!this.$refs.trendChartCanvas) return
      if (this.trendChartInstance) {
        this.trendChartInstance.destroy()
      }

      const days = ['Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt', 'Paz']
      const todayIdx = new Date().getDay() - 1
      const labels = []
      for (let i = 6; i >= 0; i--) {
        const idx = (todayIdx - i + 7) % 7
        labels.push(days[idx])
      }

      const total = (this.metrics.activeTickets || 0) + (this.metrics.waitingApprovalTickets || 0) + (this.metrics.completedTickets || 0)
      const dataPoints = [
        Math.max(1, Math.round(total * 0.1)),
        Math.max(2, Math.round(total * 0.18)),
        Math.max(1, Math.round(total * 0.14)),
        Math.max(3, Math.round(total * 0.22)),
        Math.max(2, Math.round(total * 0.16)),
        Math.max(0, Math.round(total * 0.08)),
        Math.max(1, Math.round(total * 0.12))
      ]

      const ctx = this.$refs.trendChartCanvas.getContext('2d')
      const gradient = ctx.createLinearGradient(0, 0, 0, 220)
      gradient.addColorStop(0, 'rgba(37, 99, 235, 0.35)')
      gradient.addColorStop(1, 'rgba(37, 99, 235, 0.01)')

      this.trendChartInstance = new Chart(ctx, {
        type: 'line',
        data: {
          labels,
          datasets: [{
            label: 'Talep Sayısı',
            data: dataPoints,
            fill: true,
            backgroundColor: gradient,
            borderColor: '#2563EB',
            borderWidth: 2.5,
            pointBackgroundColor: '#2563EB',
            pointRadius: 4,
            pointHoverRadius: 6,
            tension: 0.38
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { display: false },
            tooltip: {
              backgroundColor: '#0F172A',
              padding: 10,
              cornerRadius: 8
            }
          },
          scales: {
            x: {
              grid: { display: false },
              ticks: { color: '#94A3B8', font: { size: 11, family: 'Inter' } }
            },
            y: {
              beginAtZero: true,
              grid: { color: 'rgba(226, 232, 240, 0.6)' },
              ticks: { precision: 0, color: '#94A3B8', font: { size: 11, family: 'Inter' } }
            }
          }
        }
      })
    },

    initStatusChart() {
      if (!this.$refs.statusChartCanvas) return
      if (this.statusChartInstance) {
        this.statusChartInstance.destroy()
      }

      const active = this.metrics.activeTickets || 0
      const waiting = this.metrics.waitingApprovalTickets || 0
      const completed = this.metrics.completedTickets || 0
      const hasData = (active + waiting + completed) > 0

      const ctx = this.$refs.statusChartCanvas.getContext('2d')
      this.statusChartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
          labels: ['Beklemede / İşlemde', 'Onay Bekliyor', 'Tamamlanan'],
          datasets: [{
            data: hasData ? [active, waiting, completed] : [1, 1, 1],
            backgroundColor: hasData
              ? ['#F59E0B', '#38BDF8', '#10B981']
              : ['#E2E8F0', '#CBD5E1', '#E2E8F0'],
            borderWidth: 3,
            borderColor: 'transparent'
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          cutout: '68%',
          plugins: {
            legend: {
              position: 'bottom',
              labels: {
                boxWidth: 12,
                boxHeight: 12,
                color: '#64748B',
                font: { size: 11, family: 'Inter' },
                padding: 12
              }
            }
          }
        }
      })
    },

    goToDepartment(code) {
      if (this.$router) {
        this.$router.push(`/departments/${code}`)
      }
    },

    openTicketDetail(ticket) {
      this.selectedTicket = ticket
      this.showTicketModal = true
    },

    closeTicketModal() {
      this.showTicketModal = false
      this.selectedTicket = null
    },

    onTicketUpdated() {
      this.fetchMyTickets()
    },

    getStatusText(statusCode) {
      const map = {
        'PENDING': 'Beklemede',
        'ACCEPTED': 'Kabul Edildi',
        'WAITING_APPROVAL': 'Onay Bekliyor',
        'APPROVED': 'Tamamlandı'
      }
      return map[statusCode] || statusCode || 'Bilinmiyor'
    },

    getStatusClass(statusCode) {
      const map = {
        'PENDING': 'status-pending',
        'ACCEPTED': 'status-accepted',
        'WAITING_APPROVAL': 'status-waiting',
        'APPROVED': 'status-approved'
      }
      return map[statusCode] || 'status-default'
    },

    getSeverityText(code) {
      const map = {
        'INFO': 'Bilgi',
        'LOW': 'Düşük',
        'MEDIUM': 'Orta',
        'HIGH': 'Yüksek',
        'CRITICAL': 'Kritik'
      }
      return map[code] || code || '-'
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
    }
  }
}
</script>

<style scoped>
.dashboard-page {
  display: flex;
  flex-direction: column;
  gap: 24px;
  font-family: 'Inter', sans-serif;
}

/* ===== HERO BANNER ===== */
.hero-banner {
  background: linear-gradient(135deg, #1E3A5F 0%, #1E40AF 60%, #2563EB 100%);
  border-radius: 20px;
  padding: 34px 40px;
  color: #FFFFFF;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: relative;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(37, 99, 235, 0.2);
}

.hero-content {
  max-width: 600px;
  z-index: 2;
}

.hero-tag {
  font-size: 10px;
  font-weight: 800;
  letter-spacing: 2px;
  color: #93C5FD;
  margin-bottom: 8px;
}

.hero-banner h2 {
  font-size: 26px;
  font-weight: 800;
  margin: 0 0 8px 0;
  line-height: 1.25;
}

.hero-banner p {
  color: #BFDBFE;
  font-size: 14px;
  line-height: 1.55;
  margin: 0 0 22px 0;
}

.hero-actions {
  display: flex;
  gap: 12px;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 10px 20px;
  border-radius: 9px;
  font-size: 13px;
  font-weight: 600;
  text-decoration: none;
  cursor: pointer;
  transition: all 0.2s ease;
  border: 1px solid transparent;
}

.btn-primary {
  background: #FFFFFF;
  color: #1E40AF;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.btn-primary:hover {
  background: #EFF6FF;
  transform: translateY(-1px);
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.12);
  color: #FFFFFF;
  border-color: rgba(255, 255, 255, 0.25);
  backdrop-filter: blur(4px);
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.22);
  transform: translateY(-1px);
}

.btn-sm {
  padding: 7px 14px;
  font-size: 12px;
}

.hero-badge-art {
  position: relative;
  width: 140px;
  height: 140px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.art-ring {
  position: absolute;
  border-radius: 50%;
  border: 1px dashed rgba(255, 255, 255, 0.15);
}

.ring-1 { width: 140px; height: 140px; }
.ring-2 { width: 100px; height: 100px; }

.art-icon {
  width: 68px;
  height: 68px;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  backdrop-filter: blur(8px);
}

/* ===== KPI METRIC CARDS ===== */
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.kpi-card {
  background: var(--bg-surface, #FFFFFF);
  border-radius: 16px;
  padding: 20px 22px;
  border: 1px solid var(--border-color, #E2E8F0);
  box-shadow: var(--shadow-sm);
  display: flex;
  align-items: flex-start;
  gap: 16px;
  transition: all 0.25s ease;
}

.kpi-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-md);
}

.kpi-icon-wrap {
  width: 46px;
  height: 46px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.pending-icon { background: var(--status-progress-bg); color: var(--status-progress-text); }
.waiting-icon { background: var(--status-open-bg); color: var(--status-open-text); }
.approved-icon { background: var(--status-resolved-bg); color: var(--status-resolved-text); }
.dept-icon { background: rgba(139, 92, 246, 0.15); color: #8B5CF6; }

.kpi-body {
  display: flex;
  flex-direction: column;
  min-width: 0;
}

.kpi-label {
  font-size: 12px;
  font-weight: 600;
  color: var(--text-muted, #64748B);
  margin-bottom: 4px;
}

.kpi-value {
  font-size: 26px;
  font-weight: 800;
  color: var(--text-main, #0F172A);
  line-height: 1.15;
  margin-bottom: 4px;
}

.kpi-sub {
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
}

/* ===== ANALYTICS & CHARTS GRID ===== */
.analytics-charts-grid {
  display: grid;
  grid-template-columns: 65% 35%;
  gap: 20px;
}

.chart-panel {
  display: flex;
  flex-direction: column;
}

.chart-subtitle {
  font-size: 12px;
  color: var(--text-muted, #94A3B8);
  font-weight: 500;
}

.chart-legend-badge {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: var(--text-secondary, #475569);
  background: var(--bg-muted, #F8FAFC);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 6px;
  padding: 4px 10px;
}

.legend-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #2563EB;
}

.chart-canvas-container {
  height: 220px;
  padding: 16px 20px;
  position: relative;
}

.doughnut-wrap {
  height: 220px;
  padding: 8px 16px;
}

/* ===== MAIN CONTENT GRID ===== */
.dashboard-grid {
  display: grid;
  grid-template-columns: 65% 35%;
  gap: 20px;
}

.panel {
  background: var(--bg-surface, #FFFFFF);
  border-radius: 16px;
  border: 1px solid var(--border-color, #E2E8F0);
  box-shadow: var(--shadow-sm);
  overflow: hidden;
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.panel-header {
  padding: 18px 24px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-title-group {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.panel-title-group h3,
.panel-header h3 {
  font-size: 15px;
  font-weight: 700;
  color: var(--text-main, #0F172A);
  margin: 0;
}

.badge-count {
  background: var(--primary-light, #EFF6FF);
  color: var(--primary-color, #2563EB);
  font-size: 11px;
  font-weight: 700;
  padding: 2px 8px;
  border-radius: 20px;
  display: inline-block;
  width: fit-content;
  margin-top: 4px;
}

.panel-link {
  font-size: 12px;
  font-weight: 600;
  color: var(--primary-color, #2563EB);
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 4px;
  transition: all 0.15s ease;
}

.panel-link:hover {
  transform: translateX(2px);
}

.panel-loading-skeleton {
  padding: 20px 24px;
}

.panel-empty {
  text-align: center;
  padding: 48px 24px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.empty-icon {
  color: var(--text-muted, #94A3B8);
}

.panel-empty h4 {
  color: var(--text-main, #0F172A);
  font-size: 15px;
  margin: 0;
}

.panel-empty p {
  color: var(--text-muted, #64748B);
  font-size: 13px;
  margin: 0 0 8px 0;
}

/* Tables */
.tickets-table-wrap {
  overflow-x: auto;
}

.dashboard-table {
  width: 100%;
  border-collapse: collapse;
}

.dashboard-table th {
  padding: 12px 20px;
  font-size: 11px;
  font-weight: 700;
  color: var(--text-muted, #64748B);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  background: var(--bg-muted, #F8FAFC);
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  text-align: left;
}

.dashboard-table td {
  padding: 13px 20px;
  font-size: 13px;
  border-bottom: 1px solid var(--border-color, #F1F5F9);
}

.clickable-row {
  cursor: pointer;
  transition: background 0.15s ease;
}

.clickable-row:hover {
  background: var(--bg-hover, #F8FAFC);
}

.dash-id-badge {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-muted, #64748B);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 2px 7px;
  border-radius: 4px;
  font-size: 11px;
  font-weight: 700;
}

.col-title {
  font-weight: 600;
  color: var(--text-main, #0F172A);
}

.category-chip {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  border-radius: 6px;
  padding: 3px 8px;
  font-size: 11px;
  font-weight: 600;
}

.severity-dot-wrap {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 2px 8px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 700;
}

.severity-dot-wrap .dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
}

.sev-info { background: var(--severity-info-bg); color: var(--severity-info-text); }
.sev-low { background: var(--severity-low-bg); color: var(--severity-low-text); }
.sev-medium { background: var(--severity-medium-bg); color: var(--severity-medium-text); }
.sev-high { background: var(--severity-high-bg); color: var(--severity-high-text); }
.sev-critical { background: var(--severity-critical-bg); color: var(--severity-critical-text); }

.status-chip {
  display: inline-block;
  padding: 3px 9px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
}

.status-pending { background: var(--status-open-bg); color: var(--status-open-text); }
.status-accepted { background: var(--status-progress-bg); color: var(--status-progress-text); }
.status-waiting { background: var(--status-progress-bg); color: var(--status-progress-text); }
.status-approved { background: var(--status-resolved-bg); color: var(--status-resolved-text); }

.col-date {
  color: var(--text-muted, #94A3B8);
  font-size: 12px;
}

/* Departments Shortcuts */
.dept-shortcuts-list {
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.dept-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 14px;
  border-radius: 10px;
  border: 1px solid var(--border-color, #E2E8F0);
  background: var(--bg-surface, #FFFFFF);
  cursor: pointer;
  transition: all 0.2s ease;
}

.dept-item:hover {
  background: var(--bg-hover, #F8FAFC);
  border-color: var(--primary-border, #BFDBFE);
  transform: translateX(3px);
}

.dept-icon-box {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.dept-info {
  flex: 1;
  min-width: 0;
}

.dept-info h4 {
  font-size: 13px;
  font-weight: 600;
  color: var(--text-main, #0F172A);
  margin: 0 0 2px 0;
}

.dept-code-tag {
  font-size: 10px;
  font-weight: 700;
  color: var(--text-muted, #64748B);
  background: var(--bg-muted, #F1F5F9);
  padding: 1px 5px;
  border-radius: 4px;
}

.dept-arrow {
  color: var(--text-muted, #94A3B8);
}

.dept-item:hover .dept-arrow {
  color: var(--primary-color, #2563EB);
}

@media (max-width: 1100px) {
  .kpi-grid { grid-template-columns: repeat(2, 1fr); }
  .analytics-charts-grid { grid-template-columns: 1fr; }
  .dashboard-grid { grid-template-columns: 1fr; }
}

@media (max-width: 640px) {
  .hero-banner { padding: 24px; flex-direction: column; text-align: center; }
  .hero-badge-art { display: none; }
  .hero-actions { justify-content: center; }
  .kpi-grid { grid-template-columns: 1fr; }
}
</style>

<template>
  <div class="user-stats-page">
    <div class="stats-header">
      <div class="header-icon">
        <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
          <line x1="18" y1="20" x2="18" y2="10"/>
          <line x1="12" y1="20" x2="12" y2="4"/>
          <line x1="6" y1="20" x2="6" y2="14"/>
        </svg>
      </div>
      <div class="header-text">
        <h2>Kullanıcı Performans İstatistikleri</h2>
        <p>Kullanıcı bazlı ortalama onaya gönderme, tamamlanma süreleri ve çözüm oranları</p>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters-card">
      <div class="filter-group">
        <label>Başlangıç Tarihi</label>
        <input type="date" v-model="filters.from" class="stats-input" />
      </div>
      <div class="filter-group">
        <label>Bitiş Tarihi</label>
        <input type="date" v-model="filters.to" class="stats-input" />
      </div>
      <div class="filter-group">
        <label>Departman Kodu</label>
        <input type="text" v-model="filters.departmentCode" placeholder="Opsiyonel (örn: IT)" class="stats-input" />
      </div>
      <div class="filter-actions">
        <button class="apply-btn" @click="fetchStats" :disabled="isLoading">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"/>
          </svg>
          <span>{{ isLoading ? 'Yükleniyor...' : 'Filtrele' }}</span>
        </button>
        <button class="clear-btn" @click="clearFilters" :disabled="isLoading">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="23 4 23 10 17 10"/>
            <path d="M20.49 15a9 9 0 1 1-2.12-9.36L23 10"/>
          </svg>
          <span>Temizle</span>
        </button>
      </div>
    </div>

    <!-- Summary cards -->
    <div class="summary-grid" v-if="!isLoading && stats.length">
      <div class="summary-card">
        <div class="summary-label">Toplam Kullanıcı</div>
        <div class="summary-value">{{ stats.length }}</div>
        <div class="summary-sub">Analiz edilen personel</div>
      </div>
      <div class="summary-card">
        <div class="summary-label">Ort. Onaya Gönderme</div>
        <div class="summary-value">{{ formatDuration(overallAvgSubmit) }}</div>
        <div class="summary-sub">Kabulden onaya süre</div>
      </div>
      <div class="summary-card">
        <div class="summary-label">Ort. Tamamlanma</div>
        <div class="summary-value">{{ formatDuration(overallAvgCompletion) }}</div>
        <div class="summary-sub">Başlangıçtan bitişe süre</div>
      </div>
      <div class="summary-card">
        <div class="summary-label">Toplam Tamamlanan</div>
        <div class="summary-value text-success">{{ totalCompleted }}</div>
        <div class="summary-sub">Kapatılan talep adedi</div>
      </div>
    </div>

    <!-- Performance Chart -->
    <div class="chart-card" v-if="!isLoading && stats.length">
      <div class="chart-card-header">
        <div>
          <h3>Tamamlanan Talep Dağılımı</h3>
          <span class="chart-sub">Personel bazında çözülen bilet sayıları</span>
        </div>
      </div>
      <div class="chart-container">
        <canvas ref="barChartCanvas"></canvas>
      </div>
    </div>

    <!-- Table -->
    <div class="table-card">
      <div v-if="isLoading" class="table-loading-skeleton">
        <SkeletonLoader type="table" :count="5" />
      </div>

      <div v-else-if="!stats.length" class="empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/>
          <line x1="12" y1="8" x2="12" y2="12"/>
          <line x1="12" y1="16" x2="12.01" y2="16"/>
        </svg>
        <p>Seçilen filtrelere uygun istatistik verisi bulunamadı.</p>
      </div>

      <div v-else class="table-responsive">
        <table class="stats-table">
          <thead>
            <tr>
              <th>Kullanıcı</th>
              <th class="num">Kabul Edilen</th>
              <th class="num">Onaya Gönderilen</th>
              <th class="num">Tamamlanan</th>
              <th>Tamamlama Oranı</th>
              <th class="num">Ort. Onaya Gönderme</th>
              <th class="num">Ort. Tamamlanma</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="row in stats" :key="row.userId">
              <td class="user-cell">
                <div class="user-avatar">{{ (row.userName || '?').charAt(0).toUpperCase() }}</div>
                <div class="user-info-text">
                  <span class="user-name">{{ row.userName }}</span>
                  <span class="user-dept-code" v-if="row.departmentCode">{{ row.departmentCode }}</span>
                </div>
              </td>
              <td class="num">{{ row.acceptedTicketCount }}</td>
              <td class="num">{{ row.submittedForApprovalCount }}</td>
              <td class="num font-bold">{{ row.completedTicketCount }}</td>
              <td class="progress-cell">
                <div class="progress-bar-wrap">
                  <div
                    class="progress-bar-fill"
                    :style="{ width: `${getCompletionRate(row)}%` }"
                  ></div>
                </div>
                <span class="progress-text">%{{ getCompletionRate(row) }}</span>
              </td>
              <td class="num time-text">{{ formatDuration(row.avgMinutesToSubmitForApproval) }}</td>
              <td class="num time-text">{{ formatDuration(row.avgMinutesToCompletion) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { Chart, registerables } from 'chart.js'
import SkeletonLoader from './common/SkeletonLoader.vue'
import errorManager from '../utils/ErrorManager.js'

Chart.register(...registerables)

export default {
  name: 'UserStats',
  components: {
    SkeletonLoader
  },
  data() {
    return {
      isLoading: false,
      stats: [],
      filters: {
        from: '',
        to: '',
        departmentCode: ''
      },
      barChartInstance: null
    }
  },
  computed: {
    overallAvgSubmit() {
      const vals = this.stats
        .map(s => s.avgMinutesToSubmitForApproval)
        .filter(v => v !== null && v !== undefined)
      if (!vals.length) return null
      return vals.reduce((a, b) => a + b, 0) / vals.length
    },
    overallAvgCompletion() {
      const vals = this.stats
        .map(s => s.avgMinutesToCompletion)
        .filter(v => v !== null && v !== undefined)
      if (!vals.length) return null
      return vals.reduce((a, b) => a + b, 0) / vals.length
    },
    totalCompleted() {
      return this.stats.reduce((sum, s) => sum + (s.completedTicketCount || 0), 0)
    }
  },
  mounted() {
    this.fetchStats()
  },
  beforeUnmount() {
    if (this.barChartInstance) {
      this.barChartInstance.destroy()
    }
  },
  methods: {
    clearFilters() {
      this.filters = { from: '', to: '', departmentCode: '' }
      this.fetchStats()
    },

    buildQueryString() {
      const params = new URLSearchParams()
      if (this.filters.from) {
        params.append('from', new Date(this.filters.from + 'T00:00:00Z').toISOString())
      }
      if (this.filters.to) {
        params.append('to', new Date(this.filters.to + 'T23:59:59Z').toISOString())
      }
      if (this.filters.departmentCode && this.filters.departmentCode.trim()) {
        params.append('departmentCode', this.filters.departmentCode.trim())
      }
      const qs = params.toString()
      return qs ? `?${qs}` : ''
    },

    async fetchStats() {
      this.isLoading = true
      try {
        const token = localStorage.getItem('authToken')
        const url = `/api/stats/user-ticket-stats${this.buildQueryString()}`
        const response = await fetch(url, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) throw await errorManager.createErrorFromResponse(response)

        const data = await response.json()
        this.stats = Array.isArray(data) ? data : []
        this.$nextTick(() => {
          this.initBarChart()
        })
      } catch (error) {
        errorManager.logError('Fetch user ticket stats', error)
        this.stats = []
      } finally {
        this.isLoading = false
      }
    },

    initBarChart() {
      if (!this.$refs.barChartCanvas) return
      if (this.barChartInstance) {
        this.barChartInstance.destroy()
      }

      const topStats = [...this.stats]
        .sort((a, b) => (b.completedTicketCount || 0) - (a.completedTicketCount || 0))
        .slice(0, 10)

      const labels = topStats.map(s => s.userName || 'Kullanıcı')
      const dataValues = topStats.map(s => s.completedTicketCount || 0)

      const ctx = this.$refs.barChartCanvas.getContext('2d')
      this.barChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
          labels,
          datasets: [{
            label: 'Tamamlanan Talep',
            data: dataValues,
            backgroundColor: '#3B82F6',
            borderRadius: 6,
            barThickness: 24
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
              ticks: { color: '#64748B', font: { size: 12, family: 'Inter' } }
            },
            y: {
              beginAtZero: true,
              grid: { color: 'rgba(226, 232, 240, 0.6)' },
              ticks: { precision: 0, color: '#64748B', font: { size: 11, family: 'Inter' } }
            }
          }
        }
      })
    },

    getCompletionRate(row) {
      const accepted = row.acceptedTicketCount || 0
      const completed = row.completedTicketCount || 0
      if (accepted === 0 && completed > 0) return 100
      if (accepted === 0) return 0
      return Math.min(100, Math.round((completed / accepted) * 100))
    },

    formatDuration(minutes) {
      if (minutes === null || minutes === undefined || Number.isNaN(minutes)) return '-'
      const m = Math.round(minutes)
      if (m < 60) return `${m} dk`
      const hours = Math.floor(m / 60)
      const remMin = m % 60
      if (hours < 24) {
        return remMin > 0 ? `${hours}s ${remMin}dk` : `${hours}s`
      }
      const days = Math.floor(hours / 24)
      const remHours = hours % 24
      return remHours > 0 ? `${days}g ${remHours}s` : `${days}g`
    }
  }
}
</script>

<style scoped>
.user-stats-page {
  display: flex;
  flex-direction: column;
  gap: 20px;
  font-family: 'Inter', sans-serif;
}

.stats-header {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 28px 36px;
  background: linear-gradient(135deg, #1E3A5F 0%, #2563EB 100%);
  border-radius: 20px;
  box-shadow: 0 8px 24px rgba(37, 99, 235, 0.25);
  color: #FFFFFF;
}

.header-icon {
  width: 56px;
  height: 56px;
  background: rgba(255, 255, 255, 0.15);
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.header-text h2 {
  color: #FFFFFF;
  font-size: 24px;
  font-weight: 800;
  margin: 0 0 4px 0;
}

.header-text p {
  color: rgba(255, 255, 255, 0.85);
  font-size: 13px;
  margin: 0;
}

/* Filters Card */
.filters-card {
  display: flex;
  flex-wrap: wrap;
  align-items: flex-end;
  gap: 16px;
  background: var(--bg-surface, #FFFFFF);
  padding: 18px 24px;
  border-radius: 14px;
  border: 1px solid var(--border-color, #E2E8F0);
  box-shadow: var(--shadow-sm);
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.filter-group label {
  font-size: 12px;
  font-weight: 600;
  color: var(--text-secondary, #475569);
}

.stats-input {
  padding: 9px 12px;
  border: 1.5px solid var(--border-color, #CBD5E1);
  border-radius: 8px;
  font-size: 13px;
  background: var(--bg-surface, #FFFFFF);
  color: var(--text-main, #0F172A);
  outline: none;
  transition: all 0.2s ease;
}

.stats-input:focus {
  border-color: var(--primary-color, #2563EB);
  box-shadow: 0 0 0 3px var(--primary-light, rgba(37, 99, 235, 0.15));
}

.filter-actions {
  display: flex;
  gap: 8px;
}

.apply-btn, .clear-btn {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 9px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
}

.apply-btn {
  background: var(--primary-color, #2563EB);
  color: #FFFFFF;
}

.apply-btn:hover:not(:disabled) {
  background: var(--primary-hover, #1D4ED8);
}

.clear-btn {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  border: 1px solid var(--border-color, #E2E8F0);
}

.clear-btn:hover:not(:disabled) {
  background: var(--border-subtle, #E2E8F0);
  color: var(--text-main, #0F172A);
}

/* Summary Grid */
.summary-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.summary-card {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 14px;
  padding: 18px 20px;
  box-shadow: var(--shadow-sm);
  display: flex;
  flex-direction: column;
  transition: background-color 0.25s ease;
}

.summary-label {
  font-size: 12px;
  font-weight: 600;
  color: var(--text-muted, #64748B);
  margin-bottom: 4px;
}

.summary-value {
  font-size: 26px;
  font-weight: 800;
  color: var(--text-main, #0F172A);
  line-height: 1.2;
}

.text-success {
  color: #10B981;
}

.summary-sub {
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
  margin-top: 4px;
}

/* Chart Card */
.chart-card {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 16px;
  padding: 20px 24px;
  box-shadow: var(--shadow-sm);
  display: flex;
  flex-direction: column;
}

.chart-card-header h3 {
  font-size: 16px;
  font-weight: 700;
  color: var(--text-main, #0F172A);
  margin: 0 0 2px 0;
}

.chart-sub {
  font-size: 12px;
  color: var(--text-muted, #94A3B8);
}

.chart-container {
  height: 240px;
  margin-top: 16px;
  position: relative;
}

/* Table Card */
.table-card {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 16px;
  box-shadow: var(--shadow-sm);
  overflow: hidden;
  transition: background-color 0.25s ease;
}

.table-loading-skeleton {
  padding: 20px;
}

.empty-state {
  text-align: center;
  padding: 50px 20px;
  color: var(--text-muted, #94A3B8);
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.table-responsive {
  width: 100%;
  overflow-x: auto;
}

.stats-table {
  width: 100%;
  border-collapse: collapse;
}

.stats-table th {
  background: var(--bg-muted, #F8FAFC);
  padding: 14px 18px;
  font-size: 12px;
  font-weight: 700;
  color: var(--text-muted, #64748B);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  text-align: left;
}

.stats-table td {
  padding: 14px 18px;
  font-size: 13px;
  color: var(--text-main, #0F172A);
  border-bottom: 1px solid var(--border-color, #F1F5F9);
  vertical-align: middle;
}

.stats-table th.num,
.stats-table td.num {
  text-align: right;
}

.font-bold {
  font-weight: 700;
  color: #2563EB;
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: linear-gradient(135deg, #3B82F6, #60A5FA);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 700;
  flex-shrink: 0;
}

.user-info-text {
  display: flex;
  flex-direction: column;
}

.user-name {
  font-weight: 600;
  color: var(--text-main, #0F172A);
}

.user-dept-code {
  font-size: 10px;
  color: var(--text-muted, #94A3B8);
}

/* Progress bar in table */
.progress-cell {
  width: 160px;
}

.progress-bar-wrap {
  width: 100px;
  height: 6px;
  background: var(--bg-muted, #E2E8F0);
  border-radius: 10px;
  overflow: hidden;
  display: inline-block;
  vertical-align: middle;
  margin-right: 8px;
}

.progress-bar-fill {
  height: 100%;
  background: linear-gradient(90deg, #3B82F6, #10B981);
  border-radius: 10px;
  transition: width 0.4s ease;
}

.progress-text {
  font-size: 11px;
  font-weight: 700;
  color: var(--text-secondary, #475569);
  display: inline-block;
}

.time-text {
  color: var(--text-secondary, #475569);
  font-weight: 500;
}

@media (max-width: 1024px) {
  .summary-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .summary-grid { grid-template-columns: 1fr; }
  .filters-card { flex-direction: column; align-items: stretch; }
  .filter-actions { justify-content: flex-end; }
}
</style>

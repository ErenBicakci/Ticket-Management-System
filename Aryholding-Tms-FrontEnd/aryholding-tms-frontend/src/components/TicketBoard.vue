<template>
  <div class="ticket-board">
    <!-- Top Bar: Filters + Search + View Mode Switcher -->
    <div class="board-controls-card">
      <div class="controls-top-row">
        <!-- Search Bar with Shortcut -->
        <div class="search-input-wrapper">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round" class="search-icon">
            <circle cx="11" cy="11" r="8"/>
            <line x1="21" y1="21" x2="16.65" y2="16.65"/>
          </svg>
          <input
            ref="searchInputRef"
            type="text"
            v-model="filters.search"
            placeholder="Taleplerde ara (Başlık, ID, Açıklama)..."
            class="search-input"
          />
          <button v-if="filters.search" class="clear-search-btn" @click="filters.search = ''">
            <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
          <kbd class="search-kbd">Ctrl K</kbd>
        </div>

        <!-- View Mode Segmented Control (Kanban vs Table) -->
        <div class="view-mode-toggle">
          <button
            type="button"
            class="view-btn"
            :class="{ active: viewMode === 'kanban' }"
            @click="setViewMode('kanban')"
            title="Kanban Panosu Görünümü"
          >
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <rect x="3" y="3" width="7" height="18" rx="1"/>
              <rect x="14" y="3" width="7" height="11" rx="1"/>
            </svg>
            <span>Kanban</span>
          </button>
          <button
            type="button"
            class="view-btn"
            :class="{ active: viewMode === 'table' }"
            @click="setViewMode('table')"
            title="Tablo / Liste Görünümü"
          >
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <line x1="8" y1="6" x2="21" y2="6"/>
              <line x1="8" y1="12" x2="21" y2="12"/>
              <line x1="8" y1="18" x2="21" y2="18"/>
              <line x1="3" y1="6" x2="3.01" y2="6"/>
              <line x1="3" y1="12" x2="3.01" y2="12"/>
              <line x1="3" y1="18" x2="3.01" y2="18"/>
            </svg>
            <span>Tablo</span>
          </button>
        </div>
      </div>

      <!-- Filters Dropdowns -->
      <div class="filters-row">
        <!-- Severity -->
        <div class="filter-pill-group">
          <label>Önem Derecesi:</label>
          <select v-model="filters.severityCode" @change="applyFilters" :disabled="severitiesLoading" class="pill-select">
            <option value="">{{ severitiesLoading ? 'Yükleniyor...' : 'Tümü' }}</option>
            <option v-for="severity in severities" :key="severity.code" :value="severity.code">
              {{ severity.name || severity.code }}
            </option>
          </select>
        </div>

        <!-- Category -->
        <div class="filter-pill-group">
          <label>Kategori:</label>
          <select v-model="filters.categoryCode" @change="applyFilters" :disabled="categoriesLoading" class="pill-select">
            <option value="">{{ categoriesLoading ? 'Yükleniyor...' : 'Tümü' }}</option>
            <option v-for="category in categories" :key="category.code" :value="category.code">
              {{ category.name || category.code }}
            </option>
          </select>
        </div>

        <!-- Assigned User -->
        <div class="filter-pill-group" v-if="additionalParams && additionalParams.departmentCode">
          <label>Atanan:</label>
          <select v-model="filters.assignedUser" @change="applyFilters" :disabled="usersLoading" class="pill-select">
            <option value="">{{ usersLoading ? 'Yükleniyor...' : 'Tümü' }}</option>
            <option v-for="(user, i) in departmentUsers" :key="user.username || i" :value="user.username">
              {{ user.fullName || user.username }}
            </option>
          </select>
        </div>

        <!-- Sort Order -->
        <div class="filter-pill-group">
          <label>Sıralama:</label>
          <select v-model="filters.orderDirection" @change="applyFilters" class="pill-select">
            <option value="desc">Yeniden Eskiye</option>
            <option value="asc">Eskiden Yeniye</option>
          </select>
        </div>

        <button @click="clearFilters" class="clear-btn" title="Filtreleri Sıfırla">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="23 4 23 10 17 10"/>
            <path d="M20.49 15a9 9 0 1 1-2.12-9.36L23 10"/>
          </svg>
          <span>Sıfırla</span>
        </button>
      </div>

      <!-- Active Filter Chips -->
      <div v-if="hasActiveFilters" class="active-chips-row">
        <span class="active-chips-title">Aktif Filtreler:</span>
        <span v-if="filters.search" class="filter-chip">
          Arama: "{{ filters.search }}"
          <button @click="filters.search = ''" class="chip-remove-btn" title="Kaldır">
            <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </span>
        <span v-if="filters.severityCode" class="filter-chip">
          Önem: {{ getSeverityLabel(filters.severityCode) }}
          <button @click="removeFilter('severityCode')" class="chip-remove-btn" title="Kaldır">
            <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </span>
        <span v-if="filters.categoryCode" class="filter-chip">
          Kategori: {{ getCategoryLabel(filters.categoryCode) }}
          <button @click="removeFilter('categoryCode')" class="chip-remove-btn" title="Kaldır">
            <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </span>
        <span v-if="filters.assignedUser" class="filter-chip">
          Atanan: {{ filters.assignedUser }}
          <button @click="removeFilter('assignedUser')" class="chip-remove-btn" title="Kaldır">
            <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </span>
        <button @click="clearFilters" class="clear-all-chips">Tümünü Temizle</button>
      </div>
    </div>

    <!-- Error State -->
    <div v-if="error" class="error-state">
      <div class="error-icon-wrap">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/>
          <line x1="15" y1="9" x2="9" y2="15"/>
          <line x1="9" y1="9" x2="15" y2="15"/>
        </svg>
      </div>
      <h3>Hata Oluştu</h3>
      <p>{{ error }}</p>
      <button @click="fetchAllTickets" class="retry-btn">Tekrar Dene</button>
    </div>

    <!-- View 1: Kanban Panosu -->
    <div v-else-if="viewMode === 'kanban'" class="kanban-board">
      <KanbanColumn
        v-for="column in filteredColumns"
        :key="column.statusCode"
        :title="column.title"
        :status-code="column.statusCode"
        :tickets="column.tickets"
        :loading="column.loading"
        :page="column.page"
        :page-size="pageSize"
        :can-edit="canEdit"
        :can-assign="canAssign"
        @page-change="handlePageChange"
        @edit-ticket="editTicket"
        @assign-ticket="assignTicket"
        @ticket-dropped="handleTicketDropped"
      />
    </div>

    <!-- View 2: Tablo / Liste Görünümü -->
    <div v-else-if="viewMode === 'table'" class="table-view-container">
      <div v-if="loading" class="table-loading-wrap">
        <SkeletonLoader type="table" :count="6" />
      </div>

      <div v-else-if="allFilteredTickets.length === 0" class="empty-table-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
          <rect x="3" y="4" width="18" height="18" rx="2"/>
          <path d="M8 9h8M8 13h6M8 17h4"/>
        </svg>
        <h3>Filtrelere uygun talep bulunamadı</h3>
        <p>Arama veya filtre kriterlerinizi değiştirip tekrar deneyebilirsiniz.</p>
      </div>

      <div v-else class="table-responsive">
        <table class="tms-data-table">
          <thead>
            <tr>
              <th style="width: 80px;">ID</th>
              <th>Başlık & Açıklama</th>
              <th>Kategori</th>
              <th>Önem Derecesi</th>
              <th>Durum</th>
              <th>Atanan</th>
              <th>Tarih</th>
              <th style="width: 100px; text-align: right;">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="ticket in allFilteredTickets"
              :key="ticket.ticketId"
              class="table-row-item"
              @click="editTicket(ticket.ticketId)"
            >
              <td class="col-id">
                <span class="id-badge">#{{ ticket.ticketId }}</span>
              </td>
              <td class="col-title-desc">
                <div class="title-cell">{{ ticket.title }}</div>
                <div class="desc-cell" v-if="ticket.description">{{ getSnippet(ticket.description) }}</div>
              </td>
              <td class="col-cat">
                <span class="cat-chip">{{ getCategoryLabel(ticket.categoryCode) }}</span>
              </td>
              <td class="col-sev">
                <span class="sev-pill" :class="`sev-${(ticket.severityCode || '').toLowerCase()}`">
                  <span class="dot"></span>
                  {{ getSeverityLabel(ticket.severityCode) }}
                </span>
              </td>
              <td class="col-stat">
                <span class="status-pill" :class="getStatusClass(ticket.ticketStatusCode)">
                  {{ getStatusLabel(ticket.ticketStatusCode) }}
                </span>
              </td>
              <td class="col-user">
                <div class="assignee-wrap" v-if="ticket.assignedUsername">
                  <div class="avatar-mini">{{ ticket.assignedUsername.charAt(0).toUpperCase() }}</div>
                  <span>{{ ticket.assignedUsername }}</span>
                </div>
                <span v-else class="unassigned-text">Atanmamış</span>
              </td>
              <td class="col-date">
                {{ formatDate(ticket.createdAt) }}
              </td>
              <td class="col-actions" @click.stop>
                <div class="actions-group">
                  <button class="table-action-btn" @click="editTicket(ticket.ticketId)" title="Detay">
                    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/>
                      <circle cx="12" cy="12" r="3"/>
                    </svg>
                  </button>
                  <button v-if="canAssign" class="table-action-btn" @click="assignTicket(ticket.ticketId)" title="Ata">
                    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
                      <circle cx="8.5" cy="7" r="4"/>
                      <line x1="20" y1="8" x2="20" y2="14"/>
                      <line x1="23" y1="11" x2="17" y2="11"/>
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Ticket Detail Modal -->
    <TicketDetailModal
      :is-visible="showTicketModal"
      :ticket="selectedTicket"
      :can-edit="canEdit"
      :can-accept="Boolean(additionalParams && additionalParams.departmentCode)"
      :department-code="additionalParams && additionalParams.departmentCode"
      @close="closeTicketModal"
      @edit-ticket="editTicket"
      @ticket-updated="handleTicketUpdate"
    />
  </div>
</template>

<script>
import KanbanColumn from './KanbanColumn.vue'
import TicketDetailModal from './TicketDetailModal.vue'
import SkeletonLoader from './common/SkeletonLoader.vue'
import { useToastStore } from '../stores/toast.js'

export default {
  name: 'TicketBoard',
  components: {
    KanbanColumn,
    TicketDetailModal,
    SkeletonLoader
  },
  props: {
    apiEndpoint: { type: String, required: true },
    additionalParams: { type: Object, default: () => ({}) },
    canEdit: { type: Boolean, default: true },
    canAssign: { type: Boolean, default: false },
    showCreateButton: { type: Boolean, default: true }
  },
  data() {
    return {
      viewMode: localStorage.getItem('tms_ticket_view') || 'kanban',
      loading: false,
      error: null,
      showTicketModal: false,
      selectedTicket: null,
      pageSize: 10,
      
      pendingTickets: [],
      acceptedTickets: [],
      waitingApprovalTickets: [],
      approvedTickets: [],
      
      pendingLoading: false,
      acceptedLoading: false,
      waitingApprovalLoading: false,
      approvedLoading: false,
      
      pendingPage: 1,
      acceptedPage: 1,
      waitingApprovalPage: 1,
      approvedPage: 1,
      
      categories: [],
      categoriesLoading: false,
      severities: [],
      severitiesLoading: false,
      departmentUsers: [],
      usersLoading: false,
      
      filters: {
        search: '',
        severityCode: '',
        categoryCode: '',
        assignedUser: '',
        orderDirection: 'desc'
      }
    }
  },
  computed: {
    hasActiveFilters() {
      return Boolean(
        this.filters.search ||
        this.filters.severityCode ||
        this.filters.categoryCode ||
        this.filters.assignedUser
      )
    },
    columns() {
      return [
        {
          title: 'Beklemede',
          statusCode: 'PENDING',
          tickets: this.pendingTickets,
          loading: this.pendingLoading,
          page: this.pendingPage
        },
        {
          title: 'Kabul Edildi',
          statusCode: 'ACCEPTED',
          tickets: this.acceptedTickets,
          loading: this.acceptedLoading,
          page: this.acceptedPage
        },
        {
          title: 'Onay Bekliyor',
          statusCode: 'WAITING_APPROVAL',
          tickets: this.waitingApprovalTickets,
          loading: this.waitingApprovalLoading,
          page: this.waitingApprovalPage
        },
        {
          title: 'Tamamlandı',
          statusCode: 'APPROVED',
          tickets: this.approvedTickets,
          loading: this.approvedLoading,
          page: this.approvedPage
        }
      ]
    },
    filteredColumns() {
      if (!this.filters.search.trim()) return this.columns

      const q = this.filters.search.toLowerCase().trim()
      return this.columns.map(col => ({
        ...col,
        tickets: col.tickets.filter(t => 
          (t.title && t.title.toLowerCase().includes(q)) ||
          (t.description && t.description.toLowerCase().includes(q)) ||
          String(t.ticketId).includes(q) ||
          (t.assignedUsername && t.assignedUsername.toLowerCase().includes(q))
        )
      }))
    },
    allFilteredTickets() {
      const all = [
        ...this.pendingTickets,
        ...this.acceptedTickets,
        ...this.waitingApprovalTickets,
        ...this.approvedTickets
      ]

      if (!this.filters.search.trim()) return all

      const q = this.filters.search.toLowerCase().trim()
      return all.filter(t => 
        (t.title && t.title.toLowerCase().includes(q)) ||
        (t.description && t.description.toLowerCase().includes(q)) ||
        String(t.ticketId).includes(q) ||
        (t.assignedUsername && t.assignedUsername.toLowerCase().includes(q))
      )
    }
  },
  mounted() {
    this.fetchCategories()
    this.fetchSeverities()
    this.fetchDepartmentUsers()
    this.fetchAllTickets()

    window.addEventListener('keydown', this.handleGlobalKeyDown)
  },
  beforeUnmount() {
    window.removeEventListener('keydown', this.handleGlobalKeyDown)
  },
  methods: {
    handleGlobalKeyDown(e) {
      if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault()
        if (this.$refs.searchInputRef) {
          this.$refs.searchInputRef.focus()
        }
      }
    },
    setViewMode(mode) {
      this.viewMode = mode
      localStorage.setItem('tms_ticket_view', mode)
    },
    removeFilter(key) {
      this.filters[key] = ''
      this.applyFilters()
    },
    getSnippet(text) {
      if (!text) return ''
      return text.length > 70 ? text.substring(0, 70) + '...' : text
    },
    getSeverityLabel(code) {
      const found = this.severities.find(s => s.code === code)
      if (found) return found.name || found.code
      const map = { 'INFO': 'Bilgi', 'LOW': 'Düşük', 'MEDIUM': 'Orta', 'HIGH': 'Yüksek', 'CRITICAL': 'Kritik' }
      return map[code] || code || 'Tümü'
    },
    getCategoryLabel(code) {
      const found = this.categories.find(c => c.code === code)
      if (found) return found.name || found.code
      return code || 'Tümü'
    },
    getStatusLabel(code) {
      const map = {
        'PENDING': 'Beklemede',
        'ACCEPTED': 'Kabul Edildi',
        'WAITING_APPROVAL': 'Onay Bekliyor',
        'APPROVED': 'Tamamlandı'
      }
      return map[code] || code
    },
    getStatusClass(code) {
      const map = {
        'PENDING': 'status-pending',
        'ACCEPTED': 'status-accepted',
        'WAITING_APPROVAL': 'status-waiting',
        'APPROVED': 'status-approved'
      }
      return map[code] || ''
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

    async fetchCategories() {
      this.categoriesLoading = true
      try {
        const token = localStorage.getItem('authToken')
        let url = '/api/category/get-all'
        if (this.additionalParams && this.additionalParams.departmentCode) {
          url += `?departmentCode=${this.additionalParams.departmentCode}`
        }
        const response = await fetch(url, {
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })
        if (response.ok) {
          const data = await response.json()
          const list = Array.isArray(data) ? data : (data?.items || data?.data || [])
          this.categories = list.map(c => ({
            code: c.code || c.Code || c.ticketCategoryCode || c.id,
            name: c.name || c.Name || c.ticketCategoryName || c.code
          })).filter(c => c.code)
        }
      } catch (e) {
        console.error('Categories load error:', e)
      } finally {
        this.categoriesLoading = false
      }
    },

    async fetchSeverities() {
      this.severitiesLoading = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/severity/get-all', {
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })
        if (response.ok) {
          const data = await response.json()
          const list = Array.isArray(data) ? data : (data?.items || data?.data || [])
          this.severities = list.map(s => ({
            code: s.code || s.Code || s.severityCode || s.id,
            name: s.name || s.Name || s.severityName || s.code
          })).filter(s => s.code)
        }
      } catch (e) {
        console.error('Severities load error:', e)
      } finally {
        this.severitiesLoading = false
      }
    },

    async fetchDepartmentUsers() {
      if (!this.additionalParams || !this.additionalParams.departmentCode) return
      this.usersLoading = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch(`/api/user/department-users?departmentCode=${this.additionalParams.departmentCode}`, {
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })
        if (response.ok) {
          const data = await response.json()
          this.departmentUsers = Array.isArray(data) ? data : []
        }
      } catch (e) {
        console.error('Users load error:', e)
      } finally {
        this.usersLoading = false
      }
    },

    async fetchAllTickets() {
      this.loading = true
      this.error = null
      try {
        await Promise.all([
          this.fetchTicketsByStatus('PENDING', this.pendingPage),
          this.fetchTicketsByStatus('ACCEPTED', this.acceptedPage),
          this.fetchTicketsByStatus('WAITING_APPROVAL', this.waitingApprovalPage),
          this.fetchTicketsByStatus('APPROVED', this.approvedPage)
        ])
      } catch (error) {
        this.error = 'Talepler yüklenirken bir hata oluştu.'
      } finally {
        this.loading = false
      }
    },

    setColumnLoading(statusCode, isLoading) {
      if (statusCode === 'PENDING') this.pendingLoading = isLoading
      else if (statusCode === 'ACCEPTED') this.acceptedLoading = isLoading
      else if (statusCode === 'WAITING_APPROVAL') this.waitingApprovalLoading = isLoading
      else if (statusCode === 'APPROVED') this.approvedLoading = isLoading
    },

    async fetchTicketsByStatus(statusCode, page) {
      this.setColumnLoading(statusCode, true)
      try {
        const token = localStorage.getItem('authToken')
        if (!token) return []

        const queryParams = new URLSearchParams({
          page: page.toString(),
          orderDirection: this.filters.orderDirection,
          ...this.additionalParams
        })

        if (this.filters.severityCode) queryParams.append('severityCode', this.filters.severityCode)
        if (this.filters.categoryCode) queryParams.append('categoryCode', this.filters.categoryCode)
        if (this.filters.assignedUser) queryParams.append('assignedUser', this.filters.assignedUser)
        if (statusCode) queryParams.append('statusCode', statusCode)

        const response = await fetch(`${this.apiEndpoint}?${queryParams}`, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (!response.ok) throw new Error(`HTTP error ${response.status}`)
        const data = await response.json()

        let tickets = Array.isArray(data) ? data : (data.items || [])

        if (statusCode === 'PENDING') this.pendingTickets = tickets
        else if (statusCode === 'ACCEPTED') this.acceptedTickets = tickets
        else if (statusCode === 'WAITING_APPROVAL') this.waitingApprovalTickets = tickets
        else if (statusCode === 'APPROVED') this.approvedTickets = tickets

        return tickets
      } catch (e) {
        console.error(`Error loading ${statusCode}:`, e)
        return []
      } finally {
        this.setColumnLoading(statusCode, false)
      }
    },

    async handlePageChange(statusCode, newPage) {
      if (newPage < 1) return
      await this.fetchTicketsByStatus(statusCode, newPage)
      if (statusCode === 'PENDING') this.pendingPage = newPage
      else if (statusCode === 'ACCEPTED') this.acceptedPage = newPage
      else if (statusCode === 'WAITING_APPROVAL') this.waitingApprovalPage = newPage
      else if (statusCode === 'APPROVED') this.approvedPage = newPage
    },

    applyFilters() {
      this.pendingPage = 1
      this.acceptedPage = 1
      this.waitingApprovalPage = 1
      this.approvedPage = 1
      this.fetchAllTickets()
    },

    clearFilters() {
      this.filters = {
        search: '',
        severityCode: '',
        categoryCode: '',
        assignedUser: '',
        orderDirection: 'desc'
      }
      this.applyFilters()
    },

    async handleTicketDropped({ ticketId, currentStatusCode, newStatusCode }) {
      if (currentStatusCode === newStatusCode) return

      const toastStore = useToastStore()
      const allTickets = [
        ...this.pendingTickets,
        ...this.acceptedTickets,
        ...this.waitingApprovalTickets,
        ...this.approvedTickets
      ]
      const ticket = allTickets.find(t => t.ticketId === ticketId)
      if (!ticket) return

      const removeFrom = (list) => list.filter(t => t.ticketId !== ticketId)
      if (currentStatusCode === 'PENDING') this.pendingTickets = removeFrom(this.pendingTickets)
      else if (currentStatusCode === 'ACCEPTED') this.acceptedTickets = removeFrom(this.acceptedTickets)
      else if (currentStatusCode === 'WAITING_APPROVAL') this.waitingApprovalTickets = removeFrom(this.waitingApprovalTickets)
      else if (currentStatusCode === 'APPROVED') this.approvedTickets = removeFrom(this.approvedTickets)

      ticket.ticketStatusCode = newStatusCode

      if (newStatusCode === 'PENDING') this.pendingTickets.unshift(ticket)
      else if (newStatusCode === 'ACCEPTED') this.acceptedTickets.unshift(ticket)
      else if (newStatusCode === 'WAITING_APPROVAL') this.waitingApprovalTickets.unshift(ticket)
      else if (newStatusCode === 'APPROVED') this.approvedTickets.unshift(ticket)

      toastStore.success(
        `Talep #${ticketId} durumu "${this.getStatusLabel(newStatusCode)}" olarak güncellendi.`,
        'Durum Güncellendi'
      )

      this.$emit('ticket-updated', { ticketId, ticketStatusCode: newStatusCode })
    },

    viewTicket(ticketId) {
      const all = [
        ...this.pendingTickets,
        ...this.acceptedTickets,
        ...this.waitingApprovalTickets,
        ...this.approvedTickets
      ]
      this.selectedTicket = all.find(t => t.ticketId === ticketId)
      this.showTicketModal = true
    },

    closeTicketModal() {
      this.showTicketModal = false
      this.selectedTicket = null
    },

    editTicket(ticketId) {
      this.viewTicket(ticketId)
    },

    assignTicket(ticketId) {
      this.$emit('assign-ticket', ticketId)
    },

    handleTicketUpdate(updatedData) {
      const id = updatedData.ticketId || updatedData.id
      const all = [
        ...this.pendingTickets,
        ...this.acceptedTickets,
        ...this.waitingApprovalTickets,
        ...this.approvedTickets
      ]
      const t = all.find(x => x.ticketId === id)
      if (t) {
        Object.assign(t, updatedData)
      }
      this.closeTicketModal()
      this.$emit('ticket-updated', updatedData)
      this.fetchAllTickets()
    }
  }
}
</script>

<style scoped>
.ticket-board {
  display: flex;
  flex-direction: column;
  gap: 20px;
  width: 100%;
  font-family: 'Inter', sans-serif;
}

/* Controls Card */
.board-controls-card {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 16px;
  padding: 18px 22px;
  box-shadow: var(--shadow-sm);
  display: flex;
  flex-direction: column;
  gap: 14px;
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.controls-top-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
}

/* Search Bar */
.search-input-wrapper {
  position: relative;
  flex: 1;
  min-width: 260px;
  max-width: 520px;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  left: 12px;
  color: var(--text-muted, #94A3B8);
  pointer-events: none;
}

.search-input {
  width: 100%;
  padding: 10px 80px 10px 36px;
  background: var(--bg-muted, #F8FAFC);
  border: 1.5px solid var(--border-color, #CBD5E1);
  border-radius: 10px;
  font-size: 13px;
  color: var(--text-main, #0F172A);
  outline: none;
  transition: all 0.2s ease;
}

.search-input:focus {
  background: var(--bg-surface, #FFFFFF);
  border-color: var(--primary-color, #2563EB);
  box-shadow: 0 0 0 3px var(--primary-light, rgba(37, 99, 235, 0.15));
}

.clear-search-btn {
  position: absolute;
  right: 56px;
  background: transparent;
  border: none;
  color: var(--text-muted);
  cursor: pointer;
  padding: 4px;
  display: flex;
  align-items: center;
}

.search-kbd {
  position: absolute;
  right: 10px;
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  color: var(--text-muted, #94A3B8);
  font-size: 10px;
  font-weight: 700;
  padding: 2px 6px;
  border-radius: 5px;
  pointer-events: none;
}

/* View Mode Segmented Switch */
.view-mode-toggle {
  display: flex;
  background: var(--bg-muted, #F1F5F9);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 9px;
  padding: 3px;
  gap: 3px;
}

.view-btn {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 7px 12px;
  border-radius: 7px;
  border: none;
  background: transparent;
  color: var(--text-muted, #64748B);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s ease;
}

.view-btn:hover {
  color: var(--text-main, #0F172A);
}

.view-btn.active {
  background: var(--bg-surface, #FFFFFF);
  color: var(--primary-color, #2563EB);
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
}

/* Filters Dropdowns */
.filters-row {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.filter-pill-group {
  display: flex;
  align-items: center;
  gap: 6px;
  background: var(--bg-muted, #F8FAFC);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 8px;
  padding: 4px 10px;
}

.filter-pill-group label {
  font-size: 12px;
  font-weight: 600;
  color: var(--text-muted, #64748B);
  white-space: nowrap;
}

.pill-select {
  background: transparent;
  border: none;
  outline: none;
  font-size: 12px;
  font-weight: 600;
  color: var(--text-main, #0F172A);
  cursor: pointer;
}

.clear-btn {
  display: flex;
  align-items: center;
  gap: 5px;
  background: var(--bg-muted, #F1F5F9);
  border: 1px solid var(--border-color, #E2E8F0);
  color: var(--text-secondary, #475569);
  padding: 6px 12px;
  border-radius: 8px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.clear-btn:hover {
  background: var(--border-subtle, #E2E8F0);
  color: var(--text-main, #0F172A);
}

/* Active Chips */
.active-chips-row {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
  padding-top: 6px;
  border-top: 1px solid var(--border-color, #F1F5F9);
}

.active-chips-title {
  font-size: 11px;
  font-weight: 700;
  color: var(--text-muted, #94A3B8);
  text-transform: uppercase;
}

.filter-chip {
  background: var(--primary-light, #EFF6FF);
  color: var(--primary-color, #2563EB);
  border: 1px solid var(--primary-border, #BFDBFE);
  border-radius: 20px;
  padding: 3px 10px;
  font-size: 11px;
  font-weight: 600;
  display: inline-flex;
  align-items: center;
  gap: 6px;
}

.chip-remove-btn {
  background: transparent;
  border: none;
  color: var(--primary-color, #2563EB);
  cursor: pointer;
  font-size: 11px;
  padding: 0;
  font-weight: 700;
}

.clear-all-chips {
  background: transparent;
  border: none;
  color: var(--text-muted, #64748B);
  font-size: 11px;
  font-weight: 600;
  text-decoration: underline;
  cursor: pointer;
}

/* Kanban Board Layout */
.kanban-board {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
  align-items: start;
}

/* Table View Layout */
.table-view-container {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 16px;
  box-shadow: var(--shadow-sm);
  overflow: hidden;
  transition: background-color 0.25s ease;
}

.table-loading-wrap {
  padding: 20px;
}

.empty-table-state {
  text-align: center;
  padding: 60px 20px;
  color: var(--text-muted, #94A3B8);
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.empty-table-state h3 {
  color: var(--text-main, #0F172A);
  font-size: 16px;
  margin: 0;
}

.empty-table-state p {
  color: var(--text-muted, #64748B);
  font-size: 13px;
  margin: 0;
}

.table-responsive {
  width: 100%;
  overflow-x: auto;
}

.tms-data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.tms-data-table thead {
  background: var(--bg-muted, #F8FAFC);
  border-bottom: 1px solid var(--border-color, #E2E8F0);
}

.tms-data-table th {
  padding: 14px 18px;
  font-size: 12px;
  font-weight: 700;
  color: var(--text-muted, #64748B);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.tms-data-table tbody tr {
  border-bottom: 1px solid var(--border-color, #F1F5F9);
  transition: background-color 0.15s ease;
  cursor: pointer;
}

.tms-data-table tbody tr:hover {
  background: var(--bg-hover, #F8FAFC);
}

.tms-data-table td {
  padding: 14px 18px;
  font-size: 13px;
  color: var(--text-main, #0F172A);
  vertical-align: middle;
}

.id-badge {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-muted, #64748B);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 2px 7px;
  border-radius: 4px;
  font-size: 11px;
  font-weight: 700;
}

.title-cell {
  font-weight: 700;
  color: var(--text-main, #0F172A);
  margin-bottom: 2px;
}

.desc-cell {
  font-size: 12px;
  color: var(--text-muted, #64748B);
}

.cat-chip {
  background: var(--bg-muted, #F1F5F9);
  color: var(--text-secondary, #475569);
  padding: 3px 8px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 600;
}

/* Severity Pill */
.sev-pill {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 3px 9px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 700;
  border: 1px solid transparent;
}

.sev-pill .dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
}

.sev-info { background: var(--severity-info-bg); color: var(--severity-info-text); border-color: var(--severity-info-border); }
.sev-low { background: var(--severity-low-bg); color: var(--severity-low-text); border-color: var(--severity-low-border); }
.sev-medium { background: var(--severity-medium-bg); color: var(--severity-medium-text); border-color: var(--severity-medium-border); }
.sev-high { background: var(--severity-high-bg); color: var(--severity-high-text); border-color: var(--severity-high-border); }
.sev-critical { background: var(--severity-critical-bg); color: var(--severity-critical-text); border-color: var(--severity-critical-border); }

/* Status Pill */
.status-pill {
  display: inline-block;
  padding: 3px 9px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
  border: 1px solid transparent;
}

.status-pending { background: var(--status-open-bg); color: var(--status-open-text); border-color: var(--status-open-border); }
.status-accepted { background: var(--status-progress-bg); color: var(--status-progress-text); border-color: var(--status-progress-border); }
.status-waiting { background: var(--status-progress-bg); color: var(--status-progress-text); border-color: var(--status-progress-border); }
.status-approved { background: var(--status-resolved-bg); color: var(--status-resolved-text); border-color: var(--status-resolved-border); }

.assignee-wrap {
  display: flex;
  align-items: center;
  gap: 6px;
}

.avatar-mini {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  background: linear-gradient(135deg, #3B82F6, #60A5FA);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 10px;
  font-weight: 700;
}

.unassigned-text {
  color: var(--text-muted, #94A3B8);
  font-size: 12px;
  font-style: italic;
}

.actions-group {
  display: flex;
  justify-content: flex-end;
  gap: 6px;
}

.table-action-btn {
  background: var(--bg-muted, #F1F5F9);
  border: 1px solid var(--border-color, #E2E8F0);
  color: var(--text-muted, #64748B);
  padding: 5px 8px;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.15s ease;
}

.table-action-btn:hover {
  background: var(--primary-light, #EFF6FF);
  color: var(--primary-color, #2563EB);
  border-color: var(--primary-border, #BFDBFE);
}

/* Error state */
.error-state {
  text-align: center;
  padding: 60px 20px;
  background: var(--bg-surface);
  border-radius: 16px;
  border: 1px solid var(--border-color);
}

.error-icon-wrap {
  margin-bottom: 12px;
}

.retry-btn {
  margin-top: 14px;
  background: var(--primary-color);
  color: #FFFFFF;
  border: none;
  padding: 8px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

@media (max-width: 1200px) {
  .kanban-board {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .kanban-board {
    grid-template-columns: 1fr;
  }
  .controls-top-row {
    flex-direction: column;
    align-items: stretch;
  }
  .search-input-wrapper {
    max-width: 100%;
  }
}
</style>

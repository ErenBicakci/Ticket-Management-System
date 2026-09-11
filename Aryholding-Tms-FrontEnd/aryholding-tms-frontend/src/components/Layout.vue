<template>
  <div class="dashboard-layout">
    <!-- Mobile Backdrop -->
    <div
      v-if="isMobileMenuOpen"
      class="sidebar-backdrop"
      @click="closeMobileMenu"
    ></div>

    <!-- Sidebar -->
    <aside :class="['sidebar', { 'open': isMobileMenuOpen }]">
      <div class="sidebar-header">
        <div class="logo-container" @click="navigateTo('/dashboard')">
          <div class="logo-icon">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
              <rect width="32" height="32" rx="8" fill="rgba(255,255,255,0.15)"/>
              <path d="M8 10h16M8 16h10M8 22h13" stroke="white" stroke-width="2.5" stroke-linecap="round"/>
              <circle cx="24" cy="22" r="3" fill="#60A5FA"/>
            </svg>
          </div>
          <div class="logo-text-main">BPM TOOL</div>
        </div>
        <button class="mobile-close-btn" @click="closeMobileMenu" aria-label="Menüyü Kapat">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>
      
      <nav class="sidebar-nav">
        <div class="nav-section">
          <h4 class="nav-title">ANA MENÜ</h4>
          <ul class="nav-list">
            <!-- Dashboard / Ana Sayfa -->
            <li
              class="nav-item"
              :class="{ active: isRouteActive('/dashboard') }"
              @click="navigateTo('/dashboard')"
            >
              <div class="nav-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
                  <polyline points="9 22 9 12 15 12 15 22"/>
                </svg>
              </div>
              <span>Ana Sayfa</span>
            </li>

            <!-- Talep Oluştur -->
            <li
              class="nav-item"
              :class="{ active: isRouteActive('/tickets/new') }"
              @click="navigateTo('/tickets/new')"
            >
              <div class="nav-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 5v14M5 12h14"/>
                </svg>
              </div>
              <span>Talep Oluştur</span>
            </li>

            <!-- Taleplerim -->
            <li
              class="nav-item"
              :class="{ active: isRouteActive('/tickets/my') }"
              @click="navigateTo('/tickets/my')"
            >
              <div class="nav-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <rect x="3" y="4" width="18" height="18" rx="2"/>
                  <path d="M8 9h8M8 13h6M8 17h4"/>
                </svg>
              </div>
              <span>Taleplerim</span>
            </li>

            <!-- Departmanlar -->
            <li
              class="nav-item"
              :class="{ active: isRouteActive('/departments') }"
              @click="navigateTo('/departments')"
            >
              <div class="nav-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 21h18M9 21V9l6-6 6 6v12M9 21H3M9 9H3v12"/>
                  <rect x="10" y="13" width="4" height="8"/>
                </svg>
              </div>
              <span>Departmanlar</span>
            </li>

            <!-- İstatistikler -->
            <li
              v-if="canViewStats"
              class="nav-item"
              :class="{ active: isRouteActive('/stats') }"
              @click="navigateTo('/stats')"
            >
              <div class="nav-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <line x1="18" y1="20" x2="18" y2="10"/>
                  <line x1="12" y1="20" x2="12" y2="4"/>
                  <line x1="6" y1="20" x2="6" y2="14"/>
                </svg>
              </div>
              <span>İstatistikler</span>
            </li>
          </ul>
        </div>
      </nav>
      
      <div class="sidebar-footer">
        <div class="user-profile">
          <div class="user-avatar">{{ displayUserInitials }}</div>
          <div class="user-details">
            <div class="user-name">{{ displayUserName }}</div>
            <div class="user-role">{{ displayUserRole }}</div>
          </div>
        </div>
        <button @click="handleLogout" class="logout-btn">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4"/>
            <polyline points="16 17 21 12 16 7"/>
            <line x1="21" y1="12" x2="9" y2="12"/>
          </svg>
          <span>Çıkış Yap</span>
        </button>
      </div>
    </aside>
    
    <!-- Main Content -->
    <div class="main-content">
      <header class="top-header">
        <div class="header-left-wrap">
          <!-- Mobile Hamburger Menu Button -->
          <button
            class="mobile-menu-toggle"
            @click="toggleMobileMenu"
            aria-label="Menüyü Aç"
          >
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
              <line x1="3" y1="12" x2="21" y2="12"/>
              <line x1="3" y1="6" x2="21" y2="6"/>
              <line x1="3" y1="18" x2="21" y2="18"/>
            </svg>
          </button>

          <div class="header-titles">
            <h1>{{ computedTitle }}</h1>
            <p>{{ computedSubtitle }}</p>
          </div>
        </div>

        <div class="header-right">
          <button 
            class="theme-toggle-btn" 
            @click="toggleTheme" 
            :title="isDark ? 'Açık Moda Geç' : 'Karanlık Moda Geç'"
            :aria-label="isDark ? 'Açık Moda Geç' : 'Karanlık Moda Geç'"
          >
            <!-- Sun icon when dark -->
            <svg v-if="isDark" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="5"/>
              <line x1="12" y1="1" x2="12" y2="3"/>
              <line x1="12" y1="21" x2="12" y2="23"/>
              <line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/>
              <line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/>
              <line x1="1" y1="12" x2="3" y2="12"/>
              <line x1="21" y1="12" x2="23" y2="12"/>
              <line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/>
              <line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
            </svg>
            <!-- Moon icon when light -->
            <svg v-else width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
            </svg>
          </button>

          <div class="date-time">
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="10"/>
              <polyline points="12 6 12 12 16 14"/>
            </svg>
            <span>{{ currentDate }}</span>
          </div>
        </div>
      </header>
      
      <main class="content-area">
        <slot></slot>
      </main>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '../stores/auth.js'
import { useToastStore } from '../stores/toast.js'
import { useThemeStore } from '../stores/theme.js'

export default {
  name: 'AppLayout',
  props: {
    currentPage: {
      type: String,
      default: ''
    },
    pageTitle: {
      type: String,
      default: ''
    },
    pageSubtitle: {
      type: String,
      default: ''
    },
    userInfo: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      currentDate: '',
      isMobileMenuOpen: false,
      dateTimer: null
    }
  },
  computed: {
    authStore() {
      return useAuthStore()
    },
    themeStore() {
      return useThemeStore()
    },
    isDark() {
      return this.themeStore.isDark
    },
    displayUserName() {
      if (this.userInfo && this.userInfo.userName) return this.userInfo.userName
      return this.authStore.userName || 'Kullanıcı'
    },
    displayUserInitials() {
      return (this.displayUserName || 'K').charAt(0).toUpperCase()
    },
    displayUserRole() {
      if (this.userInfo && this.userInfo.departmentCode && this.userInfo.departmentCode[0]) {
        return this.userInfo.departmentCode[0]
      }
      return this.authStore.primaryDepartment || 'Kullanıcı'
    },
    canViewStats() {
      if (this.userInfo) {
        if (this.userInfo.superUser === true) return true
        const priorities = Array.isArray(this.userInfo.priority) ? this.userInfo.priority : []
        if (priorities.some(p => parseInt(p, 10) >= 7)) return true
      }
      return this.authStore.canViewStats
    },
    computedTitle() {
      if (this.$route && this.$route.meta && this.$route.meta.title) {
        return this.$route.meta.title
      }
      return this.pageTitle || 'Dashboard'
    },
    computedSubtitle() {
      if (this.$route && this.$route.meta && this.$route.meta.subtitle) {
        return this.$route.meta.subtitle
      }
      return this.pageSubtitle || 'Hoş geldiniz'
    }
  },
  mounted() {
    this.updateCurrentDate()
    this.dateTimer = setInterval(this.updateCurrentDate, 60000)
  },
  beforeUnmount() {
    if (this.dateTimer) clearInterval(this.dateTimer)
  },
  methods: {
    updateCurrentDate() {
      const now = new Date()
      const options = { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }
      this.currentDate = now.toLocaleDateString('tr-TR', options)
    },

    toggleMobileMenu() {
      this.isMobileMenuOpen = !this.isMobileMenuOpen
    },

    closeMobileMenu() {
      this.isMobileMenuOpen = false
    },

    isRouteActive(path) {
      if (this.$route) {
        if (path === '/dashboard') {
          return this.$route.path === '/dashboard' || this.$route.path === '/'
        }
        return this.$route.path.startsWith(path)
      }
      return false
    },
    
    navigateTo(path) {
      this.closeMobileMenu()
      if (this.$router) {
        this.$router.push(path)
      }
      // For legacy event compatibility
      const pageMap = {
        '/dashboard': 'dashboard',
        '/tickets/new': 'new-ticket',
        '/tickets/my': 'my-tickets',
        '/departments': 'departments',
        '/stats': 'user-stats'
      }
      this.$emit('navigate', pageMap[path] || path)
    },
    
    toggleTheme() {
      this.themeStore.toggleTheme()
    },
    
    handleLogout() {
      this.closeMobileMenu()
      const toastStore = useToastStore()
      this.authStore.logout()
      toastStore.info('Başarıyla çıkış yapıldı.', 'Görüşmek Üzere')
      
      this.$emit('logout')
      if (this.$router) {
        this.$router.push('/login')
      }
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

* {
  font-family: 'Inter', sans-serif;
  box-sizing: border-box;
}

.dashboard-layout {
  display: flex;
  min-height: 100vh;
  background: var(--bg-body);
  position: relative;
}

/* ===== MOBILE BACKDROP ===== */
.sidebar-backdrop {
  display: none;
}

@media (max-width: 768px) {
  .sidebar-backdrop {
    display: block;
    position: fixed;
    inset: 0;
    background: rgba(15, 23, 42, 0.55);
    backdrop-filter: blur(3px);
    z-index: 150;
    animation: fadeIn 0.25s ease-out;
  }
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

/* ===== SIDEBAR ===== */
.sidebar {
  width: 260px;
  background: linear-gradient(180deg, #1E3A5F 0%, #1E40AF 100%);
  display: flex;
  flex-direction: column;
  position: fixed;
  height: 100vh;
  left: 0;
  top: 0;
  z-index: 200;
  box-shadow: 4px 0 24px rgba(30, 58, 95, 0.25);
  transition: transform 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.sidebar-header {
  padding: 0;
  border-bottom: 1px solid rgba(255,255,255,0.1);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.logo-container {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 24px 20px;
  cursor: pointer;
  user-select: none;
}

.logo-icon {
  flex-shrink: 0;
}

.logo-text-main {
  font-size: 20px;
  font-weight: 800;
  color: #FFFFFF;
  letter-spacing: 3px;
  font-family: 'Inter', sans-serif;
}

.mobile-close-btn {
  display: none;
  background: transparent;
  border: none;
  color: #FFFFFF;
  padding: 8px 16px;
  cursor: pointer;
}

.sidebar-nav {
  flex: 1;
  padding: 24px 0;
  overflow-y: auto;
}

.nav-section {
  margin-bottom: 8px;
}

.nav-title {
  color: rgba(255,255,255,0.45);
  font-size: 10px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 1.5px;
  margin: 0 20px 12px 20px;
}

.nav-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.nav-item {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  cursor: pointer;
  transition: all 0.2s ease;
  margin: 2px 12px;
  border-radius: 8px;
  color: rgba(255,255,255,0.68);
  gap: 12px;
}

.nav-item:hover {
  background: rgba(255,255,255,0.1);
  color: #FFFFFF;
}

.nav-item.active {
  background: rgba(255,255,255,0.18);
  color: #FFFFFF;
  box-shadow: inset 3px 0 0 #60A5FA;
}

.nav-item.active .nav-icon {
  color: #60A5FA;
}

.nav-icon {
  width: 20px;
  height: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: color 0.2s ease;
}

.nav-item span {
  font-size: 14px;
  font-weight: 500;
}

.sidebar-footer {
  padding: 16px;
  border-top: 1px solid rgba(255,255,255,0.1);
}

.user-profile {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 12px;
  padding: 12px;
  background: rgba(255,255,255,0.08);
  border-radius: 10px;
}

.user-avatar {
  width: 38px;
  height: 38px;
  background: linear-gradient(135deg, #3B82F6, #60A5FA);
  color: #FFFFFF;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 16px;
  flex-shrink: 0;
}

.user-details {
  flex: 1;
  min-width: 0;
}

.user-name {
  font-weight: 600;
  color: #FFFFFF;
  font-size: 13px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-role {
  font-size: 11px;
  color: rgba(255,255,255,0.65);
  margin-top: 2px;
}

.logout-btn {
  width: 100%;
  background: rgba(239, 68, 68, 0.15);
  color: #FCA5A5;
  border: 1px solid rgba(239, 68, 68, 0.3);
  padding: 10px 14px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  font-family: 'Inter', sans-serif;
}

.logout-btn:hover {
  background: rgba(239, 68, 68, 0.25);
  color: #FEE2E2;
  border-color: rgba(239, 68, 68, 0.5);
}

/* ===== MAIN CONTENT ===== */
.main-content {
  flex: 1;
  margin-left: 260px;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  transition: margin-left 0.3s ease;
}

.top-header {
  background: var(--bg-surface);
  padding: 18px 36px;
  border-bottom: 1px solid var(--border-color);
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: var(--shadow-xs);
  position: sticky;
  top: 0;
  z-index: 50;
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.header-left-wrap {
  display: flex;
  align-items: center;
  gap: 16px;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 12px;
}

.theme-toggle-btn {
  background: var(--bg-muted);
  border: 1px solid var(--border-color);
  color: var(--text-secondary);
  width: 38px;
  height: 38px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
}

.theme-toggle-btn:hover {
  background: var(--border-subtle);
  color: var(--primary-color);
  border-color: var(--primary-border);
  transform: translateY(-1px);
}

.mobile-menu-toggle {
  display: none;
  background: var(--bg-muted);
  border: 1px solid var(--border-color);
  color: var(--text-main);
  border-radius: 8px;
  padding: 8px;
  cursor: pointer;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.mobile-menu-toggle:hover {
  background: var(--border-subtle);
  color: var(--text-main);
}

.header-titles h1 {
  color: var(--text-main);
  font-size: 22px;
  font-weight: 700;
  margin: 0 0 4px 0;
  font-family: 'Inter', sans-serif;
}

.header-titles p {
  color: var(--text-muted);
  font-size: 13px;
  margin: 0;
}

.date-time {
  background: var(--bg-muted);
  color: var(--text-secondary);
  padding: 9px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 8px;
  border: 1px solid var(--border-color);
}

.content-area {
  padding: 28px 36px;
  width: 100%;
  flex: 1;
}

/* ===== RESPONSIVE ===== */
@media (max-width: 1024px) {
  .sidebar { width: 240px; }
  .main-content { margin-left: 240px; }
  .top-header { padding: 16px 24px; }
  .content-area { padding: 24px; }
}

@media (max-width: 768px) {
  .sidebar {
    transform: translateX(-100%);
    width: 280px;
  }
  .sidebar.open {
    transform: translateX(0);
  }
  .mobile-close-btn {
    display: flex;
  }
  .mobile-menu-toggle {
    display: flex;
  }
  .main-content {
    margin-left: 0;
  }
  .top-header {
    padding: 14px 18px;
  }
  .header-titles h1 {
    font-size: 19px;
  }
  .date-time {
    display: none;
  }
  .content-area {
    padding: 16px;
  }
}
</style>

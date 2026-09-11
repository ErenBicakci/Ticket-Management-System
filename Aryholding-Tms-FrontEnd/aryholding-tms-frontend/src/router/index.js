import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth.js'

import UserLogin from '../components/Login.vue'
import UserSignUp from '../components/SignUp.vue'
import UserDashboard from '../components/Dashboard.vue'
import TalepForm from '../components/TalepForm.vue'
import MyTickets from '../components/MyTickets.vue'
import DepartmentsPage from '../components/DepartmentsPage.vue'
import DepartmentDashboard from '../components/DepartmentDashboard.vue'
import UserStats from '../components/UserStats.vue'

const routes = [
  {
    path: '/',
    redirect: '/dashboard'
  },
  {
    path: '/login',
    name: 'Login',
    component: UserLogin,
    meta: { guestOnly: true, title: 'Giriş Yap' }
  },
  {
    path: '/signup',
    name: 'SignUp',
    component: UserSignUp,
    meta: { guestOnly: true, title: 'Kayıt Ol' }
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: UserDashboard,
    meta: {
      requiresAuth: true,
      title: 'Ana Sayfa',
      subtitle: 'Hoş geldiniz!'
    }
  },
  {
    path: '/tickets/new',
    name: 'NewTicket',
    component: TalepForm,
    meta: {
      requiresAuth: true,
      title: 'Yeni Talep',
      subtitle: 'Yeni bir talep oluşturun.'
    }
  },
  {
    path: '/tickets/my',
    name: 'MyTickets',
    component: MyTickets,
    meta: {
      requiresAuth: true,
      title: 'Taleplerim',
      subtitle: 'Oluşturduğunuz taleplerinizi görüntüleyin.'
    }
  },
  {
    path: '/departments',
    name: 'Departments',
    component: DepartmentsPage,
    meta: {
      requiresAuth: true,
      title: 'Departmanlar',
      subtitle: 'Sistemdeki departmanları görüntüleyin.'
    }
  },
  {
    path: '/departments/:departmentId',
    name: 'DepartmentDetail',
    component: DepartmentDashboard,
    props: true,
    meta: {
      requiresAuth: true,
      title: 'Departman Paneli',
      subtitle: 'Departman talepleri ve yönetim paneli.'
    }
  },
  {
    path: '/stats',
    name: 'UserStats',
    component: UserStats,
    meta: {
      requiresAuth: true,
      requiresStatsPermission: true,
      title: 'Kullanıcı İstatistikleri',
      subtitle: 'Kullanıcı bazlı onaya gönderme ve tamamlanma süresi istatistikleri.'
    }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: {
      template: `
        <div style="text-align: center; padding: 80px 20px;">
          <h2 style="color: #1E3A5F; font-size: 32px; font-weight: 700; margin-bottom: 12px;">404 - Sayfa Bulunamadı</h2>
          <p style="color: #64748B; font-size: 16px; margin-bottom: 24px;">Aradığınız sayfa mevcut değil veya taşınmış olabilir.</p>
          <router-link to="/dashboard" style="display: inline-block; background: #2563EB; color: white; padding: 12px 24px; border-radius: 8px; text-decoration: none; font-weight: 600;">
            Ana Sayfaya Dön
          </router-link>
        </div>
      `
    },
    meta: { requiresAuth: true, title: 'Sayfa Bulunamadı' }
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  scrollBehavior() {
    return { top: 0 }
  }
})

// Navigation Guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const isAuthenticated = authStore.checkAuth()

  // Guest-only routes (login, signup)
  if (to.meta.guestOnly && isAuthenticated) {
    return next('/dashboard')
  }

  // Protected routes
  if (to.meta.requiresAuth && !isAuthenticated) {
    return next('/login')
  }

  // Permission check for stats
  if (to.meta.requiresStatsPermission && !authStore.canViewStats) {
    return next('/dashboard')
  }

  next()
})

export default router

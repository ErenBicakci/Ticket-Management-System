<template>
  <div class="login-container">
    <div class="login-left">
      <div class="brand-section">
        <div class="brand-logo">
          <svg width="40" height="40" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect width="40" height="40" rx="10" fill="rgba(255,255,255,0.15)"/>
            <path d="M10 13h20M10 20h13M10 27h16" stroke="white" stroke-width="3" stroke-linecap="round"/>
            <circle cx="29" cy="27" r="4" fill="#60A5FA"/>
          </svg>
          <span>BPM TOOL</span>
        </div>
        <h2>İş Süreçleri Yönetim Sistemi</h2>
        <p>Taleplerinizi oluşturun, takip edin ve yönetin.</p>
        <div class="feature-list">
          <div class="feature-item">
            <div class="feature-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12"/>
              </svg>
            </div>
            <span>Gerçek zamanlı talep takibi</span>
          </div>
          <div class="feature-item">
            <div class="feature-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12"/>
              </svg>
            </div>
            <span>Departman bazlı organizasyon</span>
          </div>
          <div class="feature-item">
            <div class="feature-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12"/>
              </svg>
            </div>
            <span>Kanban görünümü ile iş akışı</span>
          </div>
        </div>
      </div>
    </div>

    <div class="login-right">
      <div class="login-card">
        <div class="login-header">
          <div class="login-header-icon">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2"/>
              <circle cx="12" cy="7" r="4"/>
            </svg>
          </div>
          <h1>Giriş Yap</h1>
          <p>Hesabınıza erişmek için bilgilerinizi girin</p>
        </div>
        
        <form @submit.prevent="handleLogin" class="login-form">
          <div class="form-group">
            <label for="email">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/>
                <polyline points="22,6 12,13 2,6"/>
              </svg>
              E-posta
            </label>
            <input
              type="email"
              id="email"
              v-model="formData.email"
              placeholder="ornek@email.com"
              required
              class="form-input"
            />
          </div>
          
          <div class="form-group">
            <label for="password">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2"/>
                <path d="M7 11V7a5 5 0 0110 0v4"/>
              </svg>
              Şifre
            </label>
            <input
              type="password"
              id="password"
              v-model="formData.password"
              placeholder="Şifrenizi girin"
              required
              class="form-input"
            />
          </div>
          
          <div class="form-options">
            <label class="checkbox-container">
              <input type="checkbox" v-model="formData.rememberMe" />
              <span class="checkmark"></span>
              Beni hatırla
            </label>
            <a href="#" class="forgot-password">Şifremi unuttum</a>
          </div>
          
          <button type="submit" class="login-button" :disabled="isLoading">
            <span v-if="isLoading" class="btn-loading">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="spin-icon">
                <path d="M21 12a9 9 0 11-6.219-8.56"/>
              </svg>
              Giriş yapılıyor...
            </span>
            <span v-else>Giriş Yap</span>
          </button>
        </form>
        
        <div class="login-footer">
          <p>Hesabınız yok mu? <a href="#" class="signup-link" @click="goToSignUp">Kayıt ol</a></p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import { useAuthStore } from '../stores/auth.js'
import { useToastStore } from '../stores/toast.js'

export default {
  name: 'UserLogin',
  data() {
    return {
      formData: {
        email: '',
        password: '',
        rememberMe: false
      },
      isLoading: false
    }
  },
  methods: {
    async handleLogin() {
      if (!this.formData.email || !this.formData.password) {
        const toastStore = useToastStore()
        toastStore.warning('Lütfen e-posta ve şifrenizi girin.', 'Eksik Bilgi')
        return
      }

      this.isLoading = true
      const authStore = useAuthStore()
      const toastStore = useToastStore()
      
      try {
        const response = await fetch('/api/auth/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            email: this.formData.email,
            password: this.formData.password
          })
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        const data = await response.json()
        
        if (data.isAuthenticated) {
          const userInfo = authStore.decodeJWT(data.token)

          const roles = userInfo && userInfo.roles
          const hasRole = Array.isArray(roles) ? roles.length > 0 : Boolean(roles && String(roles).trim().length > 0)
          if (!hasRole) {
            authStore.logout()
            errorManager.showError('Giriş yapılamadı', { message: 'Kullanıcı için tanımlı bir rol bulunamadı.' })
            return
          }
          
          authStore.setAuth(data.token, userInfo)
          toastStore.success(`Tekrar hoş geldiniz, ${userInfo.userName}!`, 'Giriş Başarılı')
          
          this.$emit('login-success', {
            user: userInfo,
            formData: this.formData
          })

          if (this.$router) {
            this.$router.push('/dashboard')
          }
        } else {
          throw new Error(data.message || 'Giriş başarısız')
        }
        
      } catch (error) {
        errorManager.logError('Login', error)
        errorManager.showError('Giriş yapılamadı', error)
      } finally {
        this.isLoading = false
      }
    },
    
    goToSignUp() {
      this.$emit('go-to-signup')
      if (this.$router) {
        this.$router.push('/signup')
      }
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

.login-container {
  min-height: 100vh;
  display: flex;
  font-family: 'Inter', sans-serif;
}

/* ===== LEFT PANEL ===== */
.login-left {
  flex: 1;
  background: linear-gradient(135deg, #1E3A5F 0%, #1E40AF 60%, #2563EB 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60px 50px;
  position: relative;
  overflow: hidden;
}

.login-left::before {
  content: '';
  position: absolute;
  top: -100px;
  right: -100px;
  width: 400px;
  height: 400px;
  background: rgba(96, 165, 250, 0.1);
  border-radius: 50%;
}

.login-left::after {
  content: '';
  position: absolute;
  bottom: -80px;
  left: -80px;
  width: 300px;
  height: 300px;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 50%;
}

.brand-section {
  position: relative;
  z-index: 1;
  max-width: 380px;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 14px;
  margin-bottom: 48px;
}

.brand-logo span {
  font-size: 24px;
  font-weight: 800;
  color: #FFFFFF;
  letter-spacing: 4px;
}

.brand-section h2 {
  font-size: 32px;
  font-weight: 700;
  color: #FFFFFF;
  margin: 0 0 16px 0;
  line-height: 1.3;
}

.brand-section p {
  color: rgba(255,255,255,0.7);
  font-size: 16px;
  margin: 0 0 40px 0;
  line-height: 1.6;
}

.feature-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.feature-item {
  display: flex;
  align-items: center;
  gap: 12px;
  color: rgba(255,255,255,0.85);
  font-size: 14px;
  font-weight: 500;
}

.feature-icon {
  width: 28px;
  height: 28px;
  background: rgba(96, 165, 250, 0.2);
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #60A5FA;
  flex-shrink: 0;
}

/* ===== RIGHT PANEL ===== */
.login-right {
  width: 480px;
  background: #F8FAFC;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 50px;
}

.login-card {
  width: 100%;
  max-width: 380px;
  animation: slideUp 0.5s ease-out;
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.login-header {
  text-align: center;
  margin-bottom: 36px;
}

.login-header-icon {
  width: 64px;
  height: 64px;
  background: #EFF6FF;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px auto;
  border: 2px solid #DBEAFE;
}

.login-header h1 {
  color: #0F172A;
  font-size: 26px;
  font-weight: 700;
  margin: 0 0 8px 0;
}

.login-header p {
  color: #64748B;
  font-size: 14px;
  margin: 0;
}

.login-form {
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 6px;
  margin-bottom: 8px;
  color: #374151;
  font-weight: 600;
  font-size: 13px;
}

.form-input {
  width: 100%;
  padding: 11px 14px;
  border: 1.5px solid #E2E8F0;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.2s ease;
  box-sizing: border-box;
  background: #FFFFFF;
  color: #1E293B;
  font-family: 'Inter', sans-serif;
}

.form-input:focus {
  outline: none;
  border-color: #2563EB;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
}

.form-input::placeholder {
  color: #94A3B8;
}

.form-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  font-size: 13px;
}

.checkbox-container {
  display: flex;
  align-items: center;
  cursor: pointer;
  color: #64748B;
  gap: 6px;
}

.checkbox-container input[type="checkbox"] {
  width: 15px;
  height: 15px;
  accent-color: #2563EB;
}

.forgot-password {
  color: #2563EB;
  text-decoration: none;
  font-weight: 500;
  font-size: 13px;
}

.forgot-password:hover {
  text-decoration: underline;
}

.login-button {
  width: 100%;
  padding: 12px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  border: none;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  font-family: 'Inter', sans-serif;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.3);
}

.login-button:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 16px rgba(37, 99, 235, 0.4);
}

.login-button:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none;
}

.btn-loading {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.spin-icon {
  animation: spin 1s linear infinite;
}

.login-footer {
  text-align: center;
  padding-top: 20px;
  border-top: 1px solid #E2E8F0;
}

.login-footer p {
  color: #64748B;
  font-size: 14px;
  margin: 0;
}

.signup-link {
  color: #2563EB;
  text-decoration: none;
  font-weight: 600;
}

.signup-link:hover {
  text-decoration: underline;
}

@media (max-width: 768px) {
  .login-left { display: none; }
  .login-right { width: 100%; padding: 30px 24px; }
}
</style>

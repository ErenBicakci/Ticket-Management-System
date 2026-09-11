<template>
  <div class="signup-container">
    <div class="signup-left">
      <div class="brand-section">
        <div class="brand-logo">
          <svg width="40" height="40" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect width="40" height="40" rx="10" fill="rgba(255,255,255,0.15)"/>
            <path d="M10 13h20M10 20h13M10 27h16" stroke="white" stroke-width="3" stroke-linecap="round"/>
            <circle cx="29" cy="27" r="4" fill="#60A5FA"/>
          </svg>
          <span>BPM TOOL</span>
        </div>
        <h2>Aramıza Katılın</h2>
        <p>Hızlı kayıt olun ve iş süreçlerinizi yönetmeye başlayın.</p>
      </div>
    </div>

    <div class="signup-right">
      <div class="signup-card">
        <div class="signup-header">
          <div class="signup-header-icon">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2563EB" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M16 21v-2a4 4 0 00-4-4H6a4 4 0 00-4 4v2"/>
              <circle cx="9" cy="7" r="4"/>
              <line x1="19" y1="8" x2="19" y2="14"/>
              <line x1="22" y1="11" x2="16" y2="11"/>
            </svg>
          </div>
          <h1>Kayıt Ol</h1>
          <p>Yeni hesap oluşturmak için bilgilerinizi girin</p>
        </div>
        
        <form @submit.prevent="handleSignUp" class="signup-form">
          <div class="form-row">
            <div class="form-group">
              <label for="firstName">İsim</label>
              <input
                type="text"
                id="firstName"
                v-model="formData.firstName"
                placeholder="İsminizi girin"
                required
                class="form-input"
              />
            </div>
            
            <div class="form-group">
              <label for="lastName">Soyisim</label>
              <input
                type="text"
                id="lastName"
                v-model="formData.lastName"
                placeholder="Soyisminizi girin"
                required
                class="form-input"
              />
            </div>
          </div>
          
          <div class="form-group">
            <label for="username">Kullanıcı Adı</label>
            <input
              type="text"
              id="username"
              v-model="formData.username"
              placeholder="Kullanıcı adınızı girin"
              required
              class="form-input"
            />
          </div>
          
          <div class="form-group">
            <label for="employeeId">Çalışan ID</label>
            <input
              type="text"
              id="employeeId"
              v-model="formData.employeeId"
              placeholder="Çalışan ID'nizi girin"
              required
              class="form-input"
            />
          </div>
          
          <div class="form-group">
            <label for="email">E-posta</label>
            <input
              type="email"
              id="email"
              v-model="formData.email"
              placeholder="E-posta adresinizi girin"
              required
              class="form-input"
            />
          </div>
          
          <div class="form-row">
            <div class="form-group">
              <label for="password">Şifre</label>
              <input
                type="password"
                id="password"
                v-model="formData.password"
                placeholder="Şifrenizi girin"
                required
                class="form-input"
              />
            </div>
            
            <div class="form-group">
              <label for="confirmPassword">Şifre Tekrar</label>
              <input
                type="password"
                id="confirmPassword"
                v-model="formData.confirmPassword"
                placeholder="Tekrar girin"
                required
                class="form-input"
              />
            </div>
          </div>
          
          <div class="form-options">
            <label class="checkbox-container">
              <input type="checkbox" v-model="formData.agreeTerms" required />
              <span class="checkmark"></span>
              <span class="terms-text">
                <a href="#" class="terms-link">Kullanım şartları</a>nı ve 
                <a href="#" class="terms-link">gizlilik politikası</a>nı kabul ediyorum
              </span>
            </label>
          </div>
          
          <button type="submit" class="signup-button" :disabled="isLoading || !isFormValid">
            <span v-if="isLoading" class="btn-loading">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="spin-icon">
                <path d="M21 12a9 9 0 11-6.219-8.56"/>
              </svg>
              Kayıt yapılıyor...
            </span>
            <span v-else>Kayıt Ol</span>
          </button>
        </form>
        
        <div class="signup-footer">
          <p>Zaten hesabınız var mı? <a href="#" class="login-link" @click="goToLogin">Giriş yap</a></p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import { useToastStore } from '../stores/toast.js'

export default {
  name: 'UserSignUp',
  data() {
    return {
      formData: {
        firstName: '',
        lastName: '',
        username: '',
        employeeId: '',
        email: '',
        password: '',
        confirmPassword: '',
        agreeTerms: false
      },
      isLoading: false
    }
  },
  computed: {
    isFormValid() {
      return this.formData.firstName && 
             this.formData.lastName && 
             this.formData.username && 
             this.formData.employeeId && 
             this.formData.email && 
             this.formData.password && 
             this.formData.confirmPassword && 
             this.formData.agreeTerms &&
             this.formData.password === this.formData.confirmPassword
    }
  },
  methods: {
    async handleSignUp() {
      const toastStore = useToastStore()

      if (!this.isFormValid) {
        toastStore.warning('Lütfen tüm alanları doldurun ve şifrelerin eşleştiğinden emin olun.', 'Eksik Bilgi')
        return
      }
      
      this.isLoading = true
      
      try {
        const response = await fetch('/api/auth/register', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            firstName: this.formData.firstName,
            lastName: this.formData.lastName,
            userName: this.formData.username,
            email: this.formData.email,
            password: this.formData.password,
            employeeId: this.formData.employeeId
          })
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        const data = await response.json()
        
        toastStore.success(data.message || 'Kayıt başarılı! Şimdi giriş yapabilirsiniz.', 'Hesap Oluşturuldu')

        this.$emit('signup-success', {
          message: data.message,
          formData: this.formData
        })

        this.goToLogin()
        
      } catch (error) {
        errorManager.logError('SignUp', error)
        errorManager.showError('Kayıt yapılamadı', error)
      } finally {
        this.isLoading = false
      }
    },
    
    goToLogin() {
      this.$emit('go-to-login')
      if (this.$router) {
        this.$router.push('/login')
      }
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

.signup-container {
  min-height: 100vh;
  display: flex;
  font-family: 'Inter', sans-serif;
}

.signup-left {
  flex: 1;
  background: linear-gradient(135deg, #1E3A5F 0%, #1E40AF 60%, #2563EB 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60px 50px;
  position: relative;
  overflow: hidden;
}

.signup-left::before {
  content: '';
  position: absolute;
  top: -100px;
  right: -100px;
  width: 400px;
  height: 400px;
  background: rgba(96, 165, 250, 0.1);
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
  margin: 0;
  line-height: 1.6;
}

.signup-right {
  width: 520px;
  background: #F8FAFC;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 50px;
  overflow-y: auto;
}

.signup-card {
  width: 100%;
  max-width: 420px;
  animation: slideUp 0.5s ease-out;
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.signup-header {
  text-align: center;
  margin-bottom: 28px;
}

.signup-header-icon {
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

.signup-header h1 {
  color: #0F172A;
  font-size: 26px;
  font-weight: 700;
  margin: 0 0 8px 0;
}

.signup-header p {
  color: #64748B;
  font-size: 14px;
  margin: 0;
}

.signup-form {
  margin-bottom: 20px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.form-group {
  margin-bottom: 16px;
}

.form-group label {
  display: block;
  margin-bottom: 6px;
  color: #374151;
  font-weight: 600;
  font-size: 13px;
}

.form-input {
  width: 100%;
  padding: 10px 13px;
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
  margin-bottom: 20px;
}

.checkbox-container {
  display: flex;
  align-items: flex-start;
  cursor: pointer;
  color: #64748B;
  font-size: 13px;
  line-height: 1.4;
  gap: 8px;
}

.checkbox-container input[type="checkbox"] {
  margin-top: 2px;
  width: 15px;
  height: 15px;
  accent-color: #2563EB;
  flex-shrink: 0;
}

.terms-text { flex: 1; }

.terms-link {
  color: #2563EB;
  text-decoration: none;
  font-weight: 600;
}

.terms-link:hover { text-decoration: underline; }

.signup-button {
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

.signup-button:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 16px rgba(37, 99, 235, 0.4);
}

.signup-button:disabled {
  opacity: 0.5;
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

.spin-icon { animation: spin 1s linear infinite; }

.signup-footer {
  text-align: center;
  padding-top: 20px;
  border-top: 1px solid #E2E8F0;
}

.signup-footer p {
  color: #64748B;
  font-size: 14px;
  margin: 0;
}

.login-link {
  color: #2563EB;
  text-decoration: none;
  font-weight: 600;
}

.login-link:hover { text-decoration: underline; }

@media (max-width: 768px) {
  .signup-left { display: none; }
  .signup-right { width: 100%; padding: 30px 24px; }
  .form-row { grid-template-columns: 1fr; gap: 0; }
}
</style>

<template>
  <div v-if="visible" class="modal-backdrop" @click.self="onClose">
    <div class="modal-card">
      <div class="modal-header">
        <h3>Yeni Departman Oluştur</h3>
        <button class="close-btn" @click="onClose">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>
      <div class="modal-body">
        <form @submit.prevent="submit">
          <div class="form-group">
            <label>Departman Adı</label>
            <input
              type="text"
              v-model.trim="form.DepartmentName"
              placeholder="Örn: Güvenlik, Lojistik"
              required
              class="form-input"
            />
          </div>
          <div class="form-group">
            <label>Departman Kodu</label>
            <input
              type="text"
              v-model.trim="form.DepartmentCode"
              placeholder="Örn: SECURITY, LOGISTICS"
              required
              class="form-input"
            />
          </div>
          <div class="actions">
            <button type="button" class="btn btn-secondary" @click="onClose" :disabled="isSubmitting">İptal</button>
            <button type="submit" class="btn btn-primary" :disabled="isSubmitting">
              <span v-if="isSubmitting">Kaydediliyor…</span>
              <span v-else>Oluştur</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import { useToastStore } from '../stores/toast.js'

export default {
  name: 'DepartmentCreateModal',
  props: {
    visible: { type: Boolean, default: false }
  },
  data() {
    return {
      form: {
        DepartmentName: '',
        DepartmentCode: ''
      },
      isSubmitting: false
    }
  },
  methods: {
    onClose() {
      if (this.isSubmitting) return
      this.$emit('close')
    },
    async submit() {
      const toastStore = useToastStore()
      if (!this.form.DepartmentName || !this.form.DepartmentCode) {
        toastStore.warning('Lütfen tüm alanları doldurun.', 'Eksik Bilgi')
        return
      }
      this.isSubmitting = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/department', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          },
          body: JSON.stringify({
            DepartmentName: this.form.DepartmentName,
            DepartmentCode: this.form.DepartmentCode.toUpperCase()
          })
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        toastStore.success('Departman başarıyla oluşturuldu.', 'Başarılı')
        this.form.DepartmentName = ''
        this.form.DepartmentCode = ''
        this.$emit('created')
      } catch (error) {
        errorManager.logError('Create Department', error)
        errorManager.showError('Departman oluşturulamadı', error, {
          DepartmentName: 'Departman Adı',
          DepartmentCode: 'Departman Kodu'
        })
      } finally {
        this.isSubmitting = false
      }
    }
  }
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  font-family: 'Inter', sans-serif;
}

.modal-card {
  background: #FFFFFF;
  border: 1px solid #E2E8F0;
  border-radius: 16px;
  width: 100%;
  max-width: 480px;
  box-shadow: 0 20px 40px rgba(15, 23, 42, 0.2);
  overflow: hidden;
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 18px 24px;
  border-bottom: 1px solid #E2E8F0;
}

.modal-header h3 {
  margin: 0;
  color: #0F172A;
  font-size: 17px;
  font-weight: 700;
}

.close-btn {
  background: #F1F5F9;
  border: none;
  border-radius: 8px;
  padding: 6px;
  cursor: pointer;
  color: #64748B;
  display: flex;
  transition: all 0.2s ease;
}

.close-btn:hover {
  background: #E2E8F0;
  color: #0F172A;
}

.modal-body {
  padding: 22px 24px;
}

.form-group {
  margin-bottom: 16px;
}

.form-group label {
  display: block;
  margin-bottom: 6px;
  color: #334155;
  font-size: 13px;
  font-weight: 600;
}

.form-input {
  width: 100%;
  padding: 10px 14px;
  border: 1.5px solid #CBD5E1;
  border-radius: 8px;
  background: #FFFFFF;
  color: #0F172A;
  font-size: 14px;
  font-family: inherit;
  transition: all 0.2s ease;
}

.form-input:focus {
  outline: none;
  border-color: #2563EB;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.btn {
  padding: 10px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  border: 1px solid transparent;
}

.btn-primary {
  background: #2563EB;
  color: #FFFFFF;
}

.btn-primary:hover:not(:disabled) {
  background: #1D4ED8;
}

.btn-secondary {
  background: #F1F5F9;
  color: #475569;
  border-color: #E2E8F0;
}

.btn-secondary:hover:not(:disabled) {
  background: #E2E8F0;
  color: #0F172A;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>

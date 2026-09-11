<template>
  <div class="category-management">
    <h3>Kategori Oluştur</h3>
    <form class="category-form" @submit.prevent="submit">
      <div class="form-row">
        <label>Kategori Adı</label>
        <input type="text" v-model.trim="form.CategoryName" placeholder="Örn: Donanım" required />
      </div>
      <div class="form-row">
        <label>Kategori Kodu</label>
        <input type="text" v-model.trim="form.CategoryCode" placeholder="Örn: HARDWARE" required />
      </div>
      <div class="form-row">
        <label>Departman Kodu</label>
        <input type="text" :value="departmentCode" disabled />
      </div>
      <div class="actions">
        <button class="submit-btn" type="submit" :disabled="isSubmitting">
          {{ isSubmitting ? 'Gönderiliyor...' : 'Kategori Oluştur' }}
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import { useToastStore } from '../stores/toast.js'

export default {
  name: 'CategoryManagement',
  props: {
    departmentCode: { type: String, required: true }
  },
  data() {
    return {
      form: {
        CategoryName: '',
        CategoryCode: ''
      },
      isSubmitting: false
    }
  },
  methods: {
    async submit() {
      const toastStore = useToastStore()
      if (!this.form.CategoryName || !this.form.CategoryCode) {
        toastStore.warning('Lütfen tüm alanları doldurun.', 'Eksik Bilgi')
        return
      }
      this.isSubmitting = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/category', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          },
          body: JSON.stringify({
            CategoryName: this.form.CategoryName,
            CategoryCode: this.form.CategoryCode,
            DepartmentCode: this.departmentCode
          })
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        toastStore.success('Kategori başarıyla oluşturuldu.', 'Başarılı')
        this.form.CategoryName = ''
        this.form.CategoryCode = ''
        this.$emit('created')
      } catch (error) {
        errorManager.logError('Create Category', error)
        errorManager.showError('Kategori oluşturulamadı', error, {
          CategoryName: 'Kategori Adı',
          CategoryCode: 'Kategori Kodu',
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
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

.category-management {
  display: block;
  font-family: 'Inter', sans-serif;
}

.category-management h3 {
  color: #1E3A5F;
  font-size: 20px;
  font-weight: 700;
  margin: 0 0 20px 0;
  padding-bottom: 14px;
  border-bottom: 2px solid #EFF6FF;
}

.category-form {
  display: grid;
  grid-template-columns: 1fr;
  gap: 16px;
  max-width: 520px;
}

.form-row {
  display: grid;
  gap: 7px;
}

label {
  color: #1E3A5F;
  font-weight: 600;
  font-size: 13px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-family: 'Inter', sans-serif;
}

input {
  padding: 10px 14px;
  border: 1.5px solid #E2E8F0;
  border-radius: 8px;
  background: #F8FAFC;
  color: #0F172A;
  font-size: 14px;
  font-family: 'Inter', sans-serif;
  transition: all 0.2s ease;
}

input:focus {
  outline: none;
  border-color: #2563EB;
  background: #FFFFFF;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
}

input:disabled {
  background: #F1F5F9;
  color: #94A3B8;
  cursor: not-allowed;
}

.actions { margin-top: 8px; }

.submit-btn {
  background: linear-gradient(135deg, #1E3A5F, #2563EB);
  color: #FFFFFF;
  border: none;
  padding: 11px 22px;
  border-radius: 9px;
  font-weight: 700;
  font-size: 14px;
  cursor: pointer;
  font-family: 'Inter', sans-serif;
  transition: all 0.2s ease;
  display: inline-flex;
  align-items: center;
  gap: 8px;
}

.submit-btn:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 18px rgba(37, 99, 235, 0.35);
}

.submit-btn:disabled {
  opacity: 0.55;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}
</style>



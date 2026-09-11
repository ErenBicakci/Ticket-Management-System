<template>
  <div class="ticket-form-container">
    <div class="ticket-form-card">
      <!-- Header -->
      <div class="form-header">
        <div class="header-icon">
          <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M12 5v14M5 12h14"/>
          </svg>
        </div>
        <h1>Yeni Talep Oluştur</h1>
        <p>Sistemde yeni bir destek veya görev talebi açmak için aşağıdaki formu doldurun.</p>
      </div>

      <form @submit.prevent="handleSubmit" class="ticket-form">
        <!-- Section 1: Temel Bilgiler -->
        <div class="form-section">
          <div class="section-title">
            <div class="section-icon">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="3" y="4" width="18" height="18" rx="2"/>
                <path d="M8 9h8M8 13h6M8 17h4"/>
              </svg>
            </div>
            <div>
              <h3>Talep Detayları</h3>
              <p class="section-subtitle">Başlık ve zengin metin açıklaması ekleyin</p>
            </div>
          </div>

          <!-- Başlık -->
          <div class="form-group">
            <label for="title" class="form-label">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z"/>
              </svg>
              <span>Talep Başlığı</span>
              <span class="required-star">*</span>
            </label>
            <div class="input-container">
              <input
                type="text"
                id="title"
                v-model="formData.title"
                placeholder="Örn: Muhasebe modülü fatura aktarım hatası"
                required
                class="form-input"
              />
            </div>
          </div>

          <!-- Rich Text Açıklama -->
          <div class="form-group">
            <label class="form-label">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
                <polyline points="14 2 14 8 20 8"/>
                <line x1="16" y1="13" x2="8" y2="13"/>
                <line x1="16" y1="17" x2="8" y2="17"/>
              </svg>
              <span>Açıklama & Hata Adımları</span>
              <span class="required-star">*</span>
            </label>
            <RichTextEditor
              v-model="formData.description"
              placeholder="Talebinizi ayrıntılı olarak açıklayın. Kalın yazı, liste veya kod blokları kullanabilirsiniz..."
              :rows="6"
            />
          </div>
        </div>

        <!-- Section 2: Önem Derecesi (Visual Cards) -->
        <div class="form-section">
          <div class="section-title">
            <div class="section-icon">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/>
                <line x1="12" y1="9" x2="12" y2="13"/>
                <line x1="12" y1="17" x2="12.01" y2="17"/>
              </svg>
            </div>
            <div>
              <h3>Önem Derecesi</h3>
              <p class="section-subtitle">Talebinizin öncelik seviyesini seçin</p>
            </div>
          </div>

          <div class="severity-grid">
            <div
              v-for="sev in displaySeverities"
              :key="sev.code"
              class="severity-card"
              :class="[
                `sev-${sev.code.toLowerCase()}`,
                { 'is-selected': formData.severityCode === sev.code }
              ]"
              @click="formData.severityCode = sev.code"
            >
              <div class="severity-card-icon">
                <!-- Info Icon -->
                <svg v-if="sev.code === 'INFO'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10"/>
                  <line x1="12" y1="16" x2="12" y2="12"/>
                  <line x1="12" y1="8" x2="12.01" y2="8"/>
                </svg>
                <!-- Low Icon -->
                <svg v-else-if="sev.code === 'LOW'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/>
                  <polyline points="22 4 12 14.01 9 11.01"/>
                </svg>
                <!-- Medium Icon -->
                <svg v-else-if="sev.code === 'MEDIUM'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10"/>
                  <line x1="12" y1="8" x2="12" y2="12"/>
                  <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>
                <!-- High Icon -->
                <svg v-else-if="sev.code === 'HIGH'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>
                  <line x1="12" y1="8" x2="12" y2="12"/>
                  <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>
                <!-- Critical Icon -->
                <svg v-else width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <polygon points="7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"/>
                  <line x1="12" y1="8" x2="12" y2="12"/>
                  <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>
              </div>
              <div class="severity-card-content">
                <div class="severity-card-header">
                  <span class="severity-title">{{ sev.name }}</span>
                  <div class="selection-check" v-if="formData.severityCode === sev.code">
                    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
                      <polyline points="20 6 9 17 4 12"/>
                    </svg>
                  </div>
                </div>
                <span class="severity-desc">{{ getSeverityDescription(sev.code) }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Section 3: Kategori -->
        <div class="form-section">
          <div class="section-title">
            <div class="section-icon">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"/>
                <line x1="7" y1="7" x2="7.01" y2="7"/>
              </svg>
            </div>
            <div>
              <h3>Kategori Seçimi</h3>
              <p class="section-subtitle">Talebin ilgili olduğu kategori</p>
            </div>
          </div>

          <div v-if="categoriesLoading" class="categories-loading">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="spin-icon">
              <path d="M21 12a9 9 0 1 1-6.219-8.56"/>
            </svg>
            <span>Kategoriler yükleniyor...</span>
          </div>

          <div v-else class="category-grid">
            <div
              v-for="cat in categories"
              :key="cat.code"
              class="category-pill"
              :class="{ 'is-selected': formData.categoryCode === cat.code }"
              @click="formData.categoryCode = cat.code"
            >
              <div class="cat-indicator"></div>
              <span>{{ cat.name }}</span>
            </div>
          </div>
        </div>

        <!-- Section 4: Dosya Eki Dropzone (Görsel Destek) -->
        <div class="form-section">
          <div class="section-title">
            <div class="section-icon">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M21.44 11.05l-9.19 9.19a6 6 0 0 1-8.49-8.49l9.19-9.19a4 4 0 0 1 5.66 5.66l-9.2 9.19a2 2 0 0 1-2.83-2.83l8.49-8.48"/>
              </svg>
            </div>
            <div>
              <h3>Dosya Ekleri (İsteğe Bağlı)</h3>
              <p class="section-subtitle">Ekran görüntüsü veya log dosyaları ekleyebilirsiniz</p>
            </div>
          </div>

          <div
            class="dropzone-area"
            :class="{ 'is-dragover': isDraggingOver }"
            @dragover.prevent="isDraggingOver = true"
            @dragleave.prevent="isDraggingOver = false"
            @drop.prevent="handleFileDrop"
            @click="$refs.fileInputRef.click()"
          >
            <input
              type="file"
              ref="fileInputRef"
              multiple
              class="file-input-hidden"
              @change="handleFileSelect"
            />
            <div class="dropzone-content">
              <div class="dropzone-icon">
                <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/>
                  <polyline points="17 8 12 3 7 8"/>
                  <line x1="12" y1="3" x2="12" y2="15"/>
                </svg>
              </div>
              <div class="dropzone-text">
                <p class="dropzone-main">Dosyaları buraya sürükleyin veya <span class="highlight">gözatın</span></p>
                <p class="dropzone-sub">PNG, JPG, PDF veya TXT dosyaları (maks. 10MB)</p>
              </div>
            </div>
          </div>

          <!-- Eklenen Dosyalar Listesi -->
          <div v-if="attachedFiles.length > 0" class="attached-files-list">
            <div v-for="(file, index) in attachedFiles" :key="index" class="file-chip">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M13 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V9z"/>
                <polyline points="13 2 13 9 20 9"/>
              </svg>
              <span class="file-name">{{ file.name }}</span>
              <span class="file-size">({{ formatFileSize(file.size) }})</span>
              <button type="button" class="remove-file-btn" @click.stop="removeFile(index)" title="Dosyayı kaldır">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                  <line x1="18" y1="6" x2="6" y2="18"/>
                  <line x1="6" y1="6" x2="18" y2="18"/>
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <button type="button" @click="goBack" class="cancel-button">
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
            <span>İptal</span>
          </button>
          <button type="submit" class="submit-button" :disabled="isLoading">
            <span v-if="isLoading" class="btn-spinner">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" class="spin-icon">
                <path d="M21 12a9 9 0 1 1-6.219-8.56"/>
              </svg>
            </span>
            <span v-else>
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12"/>
              </svg>
            </span>
            <span v-if="isLoading">Talep Oluşturuluyor...</span>
            <span v-else>Talebi Kaydet & Oluştur</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import errorManager from '../utils/ErrorManager.js'
import { useToastStore } from '../stores/toast.js'
import RichTextEditor from './common/RichTextEditor.vue'

export default {
  name: 'TalepForm',
  components: {
    RichTextEditor
  },
  props: {
    departmentCode: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      formData: {
        title: '',
        description: '',
        categoryCode: '',
        severityCode: 'MEDIUM'
      },
      isLoading: false,
      categories: [],
      categoriesLoading: false,
      severities: [],
      severitiesLoading: false,
      attachedFiles: [],
      isDraggingOver: false
    }
  },
  computed: {
    displaySeverities() {
      if (this.severities && this.severities.length > 0) {
        return this.severities
      }
      return [
        { code: 'INFO', name: 'Bilgilendirme' },
        { code: 'LOW', name: 'Düşük' },
        { code: 'MEDIUM', name: 'Orta' },
        { code: 'HIGH', name: 'Yüksek' },
        { code: 'CRITICAL', name: 'Kritik' }
      ]
    }
  },
  mounted() {
    this.fetchCategories()
    this.fetchSeverities()
  },
  watch: {
    departmentCode: {
      handler() {
        this.fetchCategories()
      },
      immediate: false
    }
  },
  methods: {
    getSeverityDescription(code) {
      const descriptions = {
        INFO: 'Bilgilendirme veya genel talep',
        LOW: 'Küçük hata veya geliştirme',
        MEDIUM: 'İş akışını etkileyen standart sorun',
        HIGH: 'Önemli aksamaya neden olan hata',
        CRITICAL: 'Sistem durması, acil müdahale'
      }
      return descriptions[code] || 'Önem derecesi'
    },

    async fetchCategories() {
      this.categoriesLoading = true
      try {
        const token = localStorage.getItem('authToken')
        let url = '/api/category/get-all'
        if (this.departmentCode) {
          url += `?departmentCode=${this.departmentCode}`
        }

        const response = await fetch(url, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        const data = await response.json()
        const list = Array.isArray(data)
          ? data
          : (Array.isArray(data?.items)
              ? data.items
              : (Array.isArray(data?.data)
                  ? data.data
                  : []))

        this.categories = list.map(c => ({
          code: c.code || c.Code || c.ticketCategoryCode || c.TicketCategoryCode || c.value || c.id,
          name: c.name || c.Name || c.ticketCategoryName || c.TicketCategoryName || c.displayName || c.label || c.code || c.ticketCategoryCode
        })).filter(c => c.code)

        if (this.categories.length > 0 && !this.formData.categoryCode) {
          this.formData.categoryCode = this.categories[0].code
        }
      } catch (error) {
        errorManager.logError('Fetch categories', error)
        this.categories = [
          { code: 'TECH_SUPPORT', name: 'Teknik Destek' },
          { code: 'TECH_HELP', name: 'Teknik Yardım' },
          { code: 'OTHER', name: 'Diğer' }
        ]
        if (!this.formData.categoryCode) {
          this.formData.categoryCode = 'TECH_SUPPORT'
        }
      } finally {
        this.categoriesLoading = false
      }
    },

    async fetchSeverities() {
      this.severitiesLoading = true
      try {
        const token = localStorage.getItem('authToken')
        const response = await fetch('/api/severity/get-all', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        const data = await response.json()
        const list = Array.isArray(data)
          ? data
          : (Array.isArray(data?.items)
              ? data.items
              : (Array.isArray(data?.data)
                  ? data.data
                  : []))

        this.severities = list.map(s => ({
          code: s.code || s.Code || s.severityCode || s.SeverityCode || s.value || s.id,
          name: s.name || s.Name || s.severityName || s.SeverityName || s.displayName || s.label || s.code || s.severityCode
        })).filter(s => s.code)
      } catch (error) {
        errorManager.logError('Fetch severities', error)
        this.severities = [
          { code: 'INFO', name: 'Bilgilendirme' },
          { code: 'LOW', name: 'Düşük' },
          { code: 'MEDIUM', name: 'Orta' },
          { code: 'HIGH', name: 'Yüksek' },
          { code: 'CRITICAL', name: 'Kritik' }
        ]
      } finally {
        this.severitiesLoading = false
      }
    },

    handleFileSelect(e) {
      const files = Array.from(e.target.files || [])
      this.addFiles(files)
    },

    handleFileDrop(e) {
      this.isDraggingOver = false
      const files = Array.from(e.dataTransfer.files || [])
      this.addFiles(files)
    },

    addFiles(files) {
      for (const file of files) {
        if (file.size > 10 * 1024 * 1024) {
          const toastStore = useToastStore()
          toastStore.warning(`${file.name} 10MB boyut sınırını aşıyor.`, 'Dosya Çok Büyük')
          continue
        }
        if (!this.attachedFiles.some(f => f.name === file.name && f.size === file.size)) {
          this.attachedFiles.push(file)
        }
      }
    },

    removeFile(index) {
      this.attachedFiles.splice(index, 1)
    },

    formatFileSize(bytes) {
      if (bytes === 0) return '0 B'
      const k = 1024
      const sizes = ['B', 'KB', 'MB', 'GB']
      const i = Math.floor(Math.log(bytes) / Math.log(k))
      return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + ' ' + sizes[i]
    },

    async handleSubmit() {
      if (!this.formData.title.trim()) {
        errorManager.showError('Hata', 'Lütfen talep başlığı girin.')
        return
      }

      if (!this.formData.description.trim()) {
        errorManager.showError('Hata', 'Lütfen açıklama alanını doldurun.')
        return
      }

      if (!this.formData.categoryCode) {
        errorManager.showError('Hata', 'Lütfen bir kategori seçin.')
        return
      }

      if (!this.formData.severityCode) {
        errorManager.showError('Hata', 'Lütfen bir önem derecesi seçin.')
        return
      }

      this.isLoading = true

      try {
        const token = localStorage.getItem('authToken')
        if (!token) {
          throw new Error('Oturum bulunamadı. Lütfen tekrar giriş yapın.')
        }

        let finalDescription = this.formData.description.trim()
        if (this.attachedFiles.length > 0) {
          finalDescription += '\n\n---\n**Ekli Dosyalar:**\n' + this.attachedFiles.map(f => `- ${f.name} (${this.formatFileSize(f.size)})`).join('\n')
        }

        const response = await fetch('/api/ticket', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          },
          body: JSON.stringify({
            title: this.formData.title.trim(),
            description: finalDescription,
            categoryCode: this.formData.categoryCode,
            severityCode: this.formData.severityCode
          })
        })

        if (!response.ok) {
          throw await errorManager.createErrorFromResponse(response)
        }

        const data = await response.json()
        const toastStore = useToastStore()
        toastStore.success('Talebiniz başarıyla kaydedildi!', 'Talep Oluşturuldu')

        this.$emit('ticket-created', data)
        if (this.$router) {
          this.$router.push('/tickets/my')
        }
      } catch (error) {
        errorManager.logError('Ticket creation', error)
        errorManager.showError('Talep oluşturulamadı', error)
      } finally {
        this.isLoading = false
      }
    },

    goBack() {
      this.$emit('go-back')
      if (this.$router) {
        this.$router.push('/dashboard')
      }
    }
  }
}
</script>

<style scoped>
@keyframes spin {
  to { transform: rotate(360deg); }
}
.spin-icon { animation: spin 1s linear infinite; }

@keyframes slideUp {
  from { opacity: 0; transform: translateY(16px); }
  to { opacity: 1; transform: translateY(0); }
}

.ticket-form-container {
  width: 100%;
  font-family: 'Inter', sans-serif;
}

.ticket-form-card {
  background: var(--bg-surface);
  border-radius: 20px;
  box-shadow: var(--shadow-card);
  padding: 36px;
  width: 100%;
  max-width: 820px;
  margin: 0 auto;
  animation: slideUp 0.35s ease-out;
  border: 1px solid var(--border-color);
  position: relative;
  overflow: hidden;
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.ticket-form-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #1E40AF, #2563EB, #60A5FA, #2563EB, #1E40AF);
  background-size: 200% 100%;
  animation: shimmerLine 3s ease-in-out infinite;
}

@keyframes shimmerLine {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}

/* Header */
.form-header {
  text-align: center;
  margin-bottom: 32px;
}

.header-icon {
  width: 56px;
  height: 56px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 16px auto;
  box-shadow: 0 8px 20px rgba(37, 99, 235, 0.28);
}

.form-header h1 {
  color: var(--text-main);
  font-size: 24px;
  font-weight: 700;
  margin-bottom: 6px;
}

.form-header p {
  color: var(--text-muted);
  font-size: 14px;
  margin: 0;
}

/* Form Sections */
.form-section {
  background: var(--bg-muted);
  border-radius: 14px;
  padding: 24px;
  margin-bottom: 22px;
  border: 1px solid var(--border-color);
  transition: background-color 0.25s ease, border-color 0.25s ease;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 20px;
  padding-bottom: 14px;
  border-bottom: 1px solid var(--border-color);
}

.section-icon {
  width: 36px;
  height: 36px;
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.section-title h3 {
  color: var(--text-main);
  font-size: 16px;
  font-weight: 700;
  margin: 0 0 2px 0;
}

.section-subtitle {
  color: var(--text-muted);
  font-size: 12px;
  margin: 0;
}

.form-group {
  margin-bottom: 18px;
}

.form-group:last-child {
  margin-bottom: 0;
}

.form-label {
  display: flex;
  align-items: center;
  gap: 6px;
  margin-bottom: 8px;
  color: var(--text-secondary);
  font-weight: 600;
  font-size: 13px;
}

.required-star {
  color: #EF4444;
  font-weight: 700;
}

.form-input {
  width: 100%;
  padding: 12px 14px;
  border: 1.5px solid var(--border-color);
  border-radius: 9px;
  font-size: 14px;
  transition: all 0.2s ease;
  background: var(--bg-surface);
  color: var(--text-main);
  outline: none;
}

.form-input:focus {
  border-color: var(--primary-color);
  box-shadow: 0 0 0 3px var(--primary-light);
}

.form-input::placeholder {
  color: var(--input-placeholder);
}

/* Severity Visual Cards */
.severity-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  gap: 10px;
}

.severity-card {
  border: 1.5px solid var(--border-color);
  background: var(--bg-surface);
  border-radius: 10px;
  padding: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: flex-start;
  gap: 10px;
  position: relative;
  user-select: none;
}

.severity-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-sm);
  border-color: var(--border-focus);
}

.severity-card-icon {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.severity-card-content {
  flex: 1;
  min-width: 0;
}

.severity-card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 2px;
}

.severity-title {
  font-size: 13px;
  font-weight: 700;
  color: var(--text-main);
}

.severity-desc {
  font-size: 11px;
  color: var(--text-muted);
  line-height: 1.3;
  display: block;
}

.selection-check {
  width: 16px;
  height: 16px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #FFFFFF;
}

/* Severity Theme Accents */
.sev-info .severity-card-icon { background: var(--severity-info-bg); color: var(--severity-info-text); }
.sev-info.is-selected { border-color: #0284C7; box-shadow: 0 0 0 2px rgba(2, 132, 199, 0.2); }
.sev-info .selection-check { background: #0284C7; }

.sev-low .severity-card-icon { background: var(--severity-low-bg); color: var(--severity-low-text); }
.sev-low.is-selected { border-color: #16A34A; box-shadow: 0 0 0 2px rgba(22, 163, 74, 0.2); }
.sev-low .selection-check { background: #16A34A; }

.sev-medium .severity-card-icon { background: var(--severity-medium-bg); color: var(--severity-medium-text); }
.sev-medium.is-selected { border-color: #CA8A04; box-shadow: 0 0 0 2px rgba(202, 138, 4, 0.2); }
.sev-medium .selection-check { background: #CA8A04; }

.sev-high .severity-card-icon { background: var(--severity-high-bg); color: var(--severity-high-text); }
.sev-high.is-selected { border-color: #EA580C; box-shadow: 0 0 0 2px rgba(234, 88, 12, 0.2); }
.sev-high .selection-check { background: #EA580C; }

.sev-critical .severity-card-icon { background: var(--severity-critical-bg); color: var(--severity-critical-text); }
.sev-critical.is-selected { border-color: #DC2626; box-shadow: 0 0 0 2px rgba(220, 38, 38, 0.2); }
.sev-critical .selection-check { background: #DC2626; }

/* Category Pills */
.category-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.category-pill {
  padding: 9px 16px;
  background: var(--bg-surface);
  border: 1.5px solid var(--border-color);
  border-radius: 9px;
  color: var(--text-secondary);
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.2s ease;
  user-select: none;
}

.category-pill:hover {
  border-color: var(--primary-border);
  color: var(--text-main);
  background: var(--bg-hover);
}

.category-pill.is-selected {
  background: var(--primary-light);
  border-color: var(--primary-color);
  color: var(--primary-color);
  font-weight: 700;
}

.cat-indicator {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--text-muted);
}

.category-pill.is-selected .cat-indicator {
  background: var(--primary-color);
}

.categories-loading {
  display: flex;
  align-items: center;
  gap: 10px;
  color: var(--text-muted);
  font-size: 13px;
}

/* Dropzone */
.dropzone-area {
  border: 2px dashed var(--border-color);
  border-radius: 12px;
  padding: 24px;
  text-align: center;
  cursor: pointer;
  background: var(--bg-surface);
  transition: all 0.2s ease;
}

.dropzone-area:hover,
.dropzone-area.is-dragover {
  border-color: var(--primary-color);
  background: var(--primary-light);
}

.file-input-hidden {
  display: none;
}

.dropzone-icon {
  color: var(--text-muted);
  margin-bottom: 8px;
}

.dropzone-area:hover .dropzone-icon {
  color: var(--primary-color);
}

.dropzone-main {
  font-size: 14px;
  font-weight: 600;
  color: var(--text-main);
  margin: 0 0 4px 0;
}

.dropzone-main .highlight {
  color: var(--primary-color);
  text-decoration: underline;
}

.dropzone-sub {
  font-size: 12px;
  color: var(--text-muted);
  margin: 0;
}

.attached-files-list {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 14px;
}

.file-chip {
  display: flex;
  align-items: center;
  gap: 6px;
  background: var(--bg-surface);
  border: 1px solid var(--border-color);
  border-radius: 6px;
  padding: 6px 10px;
  font-size: 12px;
  color: var(--text-main);
}

.file-name {
  font-weight: 500;
}

.file-size {
  color: var(--text-muted);
}

.remove-file-btn {
  background: transparent;
  border: none;
  color: var(--text-muted);
  cursor: pointer;
  padding: 2px;
  display: flex;
  align-items: center;
  border-radius: 4px;
}

.remove-file-btn:hover {
  color: #EF4444;
  background: var(--severity-critical-bg);
}

/* Actions */
.form-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 28px;
  padding-top: 24px;
  border-top: 1px solid var(--border-color);
}

.cancel-button,
.submit-button {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px 24px;
  border-radius: 9px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
  font-family: 'Inter', sans-serif;
}

.cancel-button {
  background: var(--bg-muted);
  color: var(--text-secondary);
  border: 1px solid var(--border-color);
}

.cancel-button:hover {
  background: var(--bg-hover);
  color: var(--text-main);
}

.submit-button {
  background: linear-gradient(135deg, #1E40AF, #2563EB);
  color: #FFFFFF;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.3);
}

.submit-button:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 16px rgba(37, 99, 235, 0.4);
}

.submit-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.btn-spinner {
  display: flex;
  align-items: center;
}

@media (max-width: 768px) {
  .ticket-form-card { padding: 20px; }
  .severity-grid { grid-template-columns: 1fr; }
  .form-actions { flex-direction: column; }
  .cancel-button, .submit-button { width: 100%; justify-content: center; }
}
</style>

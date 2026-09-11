import { useToastStore } from '../stores/toast.js'

class ErrorManager {
  constructor() {
    this.fieldNames = {
      'email': 'E-posta',
      'password': 'Şifre',
      'firstName': 'İsim',
      'lastName': 'Soyisim',
      'userName': 'Kullanıcı Adı',
      'employeeId': 'Çalışan ID',
      
      'title': 'Başlık',
      'description': 'Açıklama',
      'categoryCode': 'Kategori',
      'categoryName': 'Kategori Adı',
      'severityCode': 'Önem Derecesi',
      'priorityLevelId': 'Kıdem Seviyesi',
      'severityId': 'Önem Derecesi'
    }
  }

  async createErrorFromResponse(response) {
    try {
      const errorData = await response.json()
      return {
        message: errorData.message || errorData.title || `HTTP error! status: ${response.status}`,
        responseData: errorData
      }
    } catch (parseError) {
      return {
        message: `HTTP error! status: ${response.status}`,
        responseData: null
      }
    }
  }

  formatApiError(error, customFieldNames = {}) {
    if (!error) return 'Bilinmeyen bir hata oluştu.'

    if (typeof error === 'string') return error

    if (error.responseData && error.responseData.errors) {
      const errorMessages = []
      
      Object.keys(error.responseData.errors).forEach(field => {
        const fieldErrors = error.responseData.errors[field]
        if (Array.isArray(fieldErrors)) {
          fieldErrors.forEach(err => {
            const fieldName = customFieldNames[field] || this.fieldNames[field] || field
            errorMessages.push(`${fieldName}: ${err}`)
          })
        }
      })
      
      if (errorMessages.length > 0) {
        return errorMessages.join('\n')
      } else {
        return error.responseData.title || error.message
      }
    }

    if (error.responseData && error.responseData.message) {
      return error.responseData.message
    }
    
    return error.message || 'Bir hata oluştu.'
  }

  showError(title, error, customFieldNames = {}) {
    const errorMessage = this.formatApiError(error, customFieldNames)
    try {
      const toastStore = useToastStore()
      toastStore.error(errorMessage, title || 'Hata')
    } catch (e) {
      console.error(`${title}: ${errorMessage}`)
    }
  }

  showSuccess(title, message) {
    try {
      const toastStore = useToastStore()
      toastStore.success(message, title || 'Başarılı')
    } catch (e) {
      console.log(`${title}: ${message}`)
    }
  }

  addFieldName(field, displayName) {
    this.fieldNames[field] = displayName
  }

  getFieldNames() {
    return { ...this.fieldNames }
  }

  logError(context, error) {
    console.error(`${context} error:`, error)
  }
}

const errorManager = new ErrorManager()

export default errorManager
export const formatApiError = (error, fieldNames) => errorManager.formatApiError(error, fieldNames)
export const createErrorFromResponse = (response) => errorManager.createErrorFromResponse(response)
export const COMMON_FIELD_NAMES = errorManager.getFieldNames()

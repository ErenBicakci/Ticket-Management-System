<template>
  <div class="toast-container" aria-live="polite">
    <transition-group name="toast-anim" tag="div" class="toast-list">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="['toast-card', `toast-${toast.type}`]"
        @click="removeToast(toast.id)"
      >
        <div class="toast-icon">
          <!-- Success -->
          <svg v-if="toast.type === 'success'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"/>
          </svg>
          <!-- Error -->
          <svg v-else-if="toast.type === 'error'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <line x1="15" y1="9" x2="9" y2="15"/>
            <line x1="9" y1="9" x2="15" y2="15"/>
          </svg>
          <!-- Warning -->
          <svg v-else-if="toast.type === 'warning'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z"/>
            <line x1="12" y1="9" x2="12" y2="13"/>
            <line x1="12" y1="17" x2="12.01" y2="17"/>
          </svg>
          <!-- Info -->
          <svg v-else width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <line x1="12" y1="16" x2="12" y2="12"/>
            <line x1="12" y1="8" x2="12.01" y2="8"/>
          </svg>
        </div>

        <div class="toast-content">
          <h4 v-if="toast.title" class="toast-title">{{ toast.title }}</h4>
          <p class="toast-message">{{ toast.message }}</p>
        </div>

        <button class="toast-close-btn" @click.stop="removeToast(toast.id)" aria-label="Kapat">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </div>
    </transition-group>
  </div>
</template>

<script>
import { computed } from 'vue'
import { useToastStore } from '../../stores/toast.js'

export default {
  name: 'ToastContainer',
  setup() {
    const toastStore = useToastStore()
    const toasts = computed(() => toastStore.toasts)

    const removeToast = (id) => {
      toastStore.removeToast(id)
    }

    return {
      toasts,
      removeToast
    }
  }
}
</script>

<style scoped>
.toast-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 999999;
  display: flex;
  flex-direction: column;
  gap: 10px;
  pointer-events: none;
  max-width: 420px;
  width: calc(100vw - 40px);
}

.toast-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.toast-card {
  pointer-events: auto;
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 14px 16px;
  background: #FFFFFF;
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(15, 23, 42, 0.16), 0 2px 6px rgba(15, 23, 42, 0.08);
  border-left: 5px solid #3B82F6;
  border-top: 1px solid #F1F5F9;
  border-right: 1px solid #F1F5F9;
  border-bottom: 1px solid #F1F5F9;
  cursor: pointer;
  transition: all 0.25s ease;
  overflow: hidden;
}

.toast-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 14px 34px rgba(15, 23, 42, 0.2);
}

/* Variants */
.toast-success {
  border-left-color: #10B981;
}
.toast-success .toast-icon {
  color: #10B981;
  background: rgba(16, 185, 129, 0.12);
}

.toast-error {
  border-left-color: #EF4444;
}
.toast-error .toast-icon {
  color: #EF4444;
  background: rgba(239, 68, 68, 0.12);
}

.toast-warning {
  border-left-color: #F59E0B;
}
.toast-warning .toast-icon {
  color: #F59E0B;
  background: rgba(245, 158, 11, 0.12);
}

.toast-info {
  border-left-color: #2563EB;
}
.toast-info .toast-icon {
  color: #2563EB;
  background: rgba(37, 99, 235, 0.12);
}

.toast-icon {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  margin-top: 1px;
}

.toast-content {
  flex: 1;
  min-width: 0;
}

.toast-title {
  margin: 0 0 3px 0;
  font-size: 14px;
  font-weight: 700;
  color: #0F172A;
  font-family: 'Inter', sans-serif;
}

.toast-message {
  margin: 0;
  font-size: 13px;
  line-height: 1.45;
  color: #475569;
  font-family: 'Inter', sans-serif;
  word-break: break-word;
  white-space: pre-line;
}

.toast-close-btn {
  background: transparent;
  border: none;
  color: #94A3B8;
  cursor: pointer;
  padding: 4px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.toast-close-btn:hover {
  color: #0F172A;
  background: #F1F5F9;
}

/* Animations */
.toast-anim-enter-active,
.toast-anim-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-anim-enter-from {
  opacity: 0;
  transform: translateX(40px) scale(0.95);
}

.toast-anim-leave-to {
  opacity: 0;
  transform: translateX(40px) scale(0.9);
}
</style>

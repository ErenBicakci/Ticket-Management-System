<template>
  <div class="comments-tab-wrap">
    <!-- State: Loading Skeleton -->
    <div v-if="loading" class="comments-skeleton">
      <SkeletonLoader type="text" :count="4" height="60px" />
    </div>

    <!-- State: Error -->
    <div v-else-if="error" class="state-box error-box">
      <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <circle cx="12" cy="12" r="10"/>
        <line x1="15" y1="9" x2="9" y2="15"/>
        <line x1="9" y1="9" x2="15" y2="15"/>
      </svg>
      <p>{{ error }}</p>
      <button @click="$emit('retry')" class="retry-btn">Tekrar Dene</button>
    </div>

    <!-- State: Comments List -->
    <div v-else class="comments-content">
      <div v-if="comments.length === 0" class="empty-comments">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round">
          <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"/>
        </svg>
        <p>Henüz yorum yapılmamış. İlk yorumu siz yazın.</p>
      </div>

      <div v-else class="comments-list" ref="commentsContainer">
        <div
          v-for="(comment, index) in comments"
          :key="comment.id || index"
          class="comment-card"
        >
          <div class="comment-author-avatar">
            {{ getUserInitials(comment.username) }}
          </div>
          <div class="comment-body">
            <div class="comment-header-row">
              <span class="comment-author-name">{{ comment.username || 'Kullanıcı' }}</span>
              <span class="comment-date">{{ formatDate(comment.createdAt) }}</span>
            </div>
            <p class="comment-text">{{ comment.comment || comment.content }}</p>
          </div>
        </div>
      </div>

      <!-- Add Comment Form -->
      <form class="comment-input-form" @submit.prevent="submitComment">
        <textarea
          v-model="commentText"
          placeholder="Yorumunuzu yazın... (Ctrl + Enter ile gönder)"
          rows="3"
          :disabled="isSubmitting"
          @keydown.ctrl.enter.prevent="submitComment"
          class="comment-textarea"
        ></textarea>
        <div class="form-actions">
          <span class="ctrl-hint">
            <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <rect x="2" y="4" width="20" height="16" rx="2"/>
              <path d="M6 8h.001M10 8h.001M14 8h.001M18 8h.001M8 12h.001M12 12h.001M16 12h.001M18 16H6"/>
            </svg>
            Ctrl + Enter ile hızlı gönder
          </span>
          <button
            type="submit"
            class="submit-comment-btn"
            :disabled="isSubmitting || !commentText.trim()"
          >
            <span v-if="isSubmitting">Gönderiliyor...</span>
            <span v-else>Yorum Ekle</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import SkeletonLoader from '../../common/SkeletonLoader.vue'

export default {
  name: 'TicketCommentsTab',
  components: {
    SkeletonLoader
  },
  props: {
    comments: {
      type: Array,
      default: () => []
    },
    loading: {
      type: Boolean,
      default: false
    },
    error: {
      type: String,
      default: null
    },
    isSubmitting: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      commentText: ''
    }
  },
  methods: {
    getUserInitials(name) {
      if (!name) return '?'
      return name.charAt(0).toUpperCase()
    },
    formatDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleDateString('tr-TR', {
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    },
    submitComment() {
      if (!this.commentText.trim() || this.isSubmitting) return
      this.$emit('submit-comment', this.commentText.trim())
      this.commentText = ''
    }
  }
}
</script>

<style scoped>
.comments-tab-wrap {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.comments-skeleton {
  padding: 12px 4px;
}

.state-box {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 10px;
  padding: 40px 20px;
  color: var(--text-muted, #64748B);
  font-size: 13px;
}

.comments-content {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.empty-comments {
  text-align: center;
  padding: 36px 16px;
  color: var(--text-muted, #94A3B8);
  font-size: 13px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.comments-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
  max-height: 360px;
  overflow-y: auto;
  padding: 4px 4px 12px 4px;
}

.comment-card {
  display: flex;
  gap: 12px;
  background: var(--bg-muted, #F8FAFC);
  border-radius: 12px;
  padding: 12px 14px;
  border: 1px solid var(--border-color, #E2E8F0);
  transition: background-color 0.2s ease, border-color 0.2s ease;
}

.comment-author-avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: var(--primary-color, #2563EB);
  color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 13px;
  font-weight: 700;
  flex-shrink: 0;
}

.comment-body {
  flex: 1;
  min-width: 0;
}

.comment-header-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 4px;
}

.comment-author-name {
  font-size: 13px;
  font-weight: 700;
  color: var(--text-main, #0F172A);
}

.comment-date {
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
}

.comment-text {
  font-size: 13px;
  color: var(--text-secondary, #334155);
  line-height: 1.5;
  margin: 0;
  white-space: pre-wrap;
  word-break: break-word;
}

.comment-input-form {
  margin-top: 14px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  border-top: 1px solid var(--border-color, #E2E8F0);
  padding-top: 14px;
}

.comment-textarea {
  width: 100%;
  padding: 10px 12px;
  border: 1.5px solid var(--border-color, #CBD5E1);
  border-radius: 10px;
  font-size: 13px;
  font-family: inherit;
  resize: vertical;
  color: var(--text-main, #0F172A);
  background: var(--bg-surface, #FFFFFF);
  outline: none;
  transition: all 0.2s ease;
}

.comment-textarea:focus {
  border-color: var(--primary-color, #2563EB);
  box-shadow: 0 0 0 3px var(--primary-light, rgba(37, 99, 235, 0.15));
}

.form-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.ctrl-hint {
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
  display: flex;
  align-items: center;
  gap: 4px;
}

.submit-comment-btn {
  background: var(--primary-color, #2563EB);
  color: #FFFFFF;
  border: none;
  border-radius: 8px;
  padding: 8px 16px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.submit-comment-btn:hover:not(:disabled) {
  background: var(--primary-hover, #1D4ED8);
  transform: translateY(-1px);
}

.submit-comment-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.retry-btn {
  background: var(--bg-surface);
  border: 1px solid var(--border-color);
  color: var(--text-main);
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 12px;
  cursor: pointer;
}
</style>

<template>
  <div class="ticket-description-box">
    <div class="description-header">
      <div class="desc-icon">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
          <polyline points="14 2 14 8 20 8"/>
          <line x1="16" y1="13" x2="8" y2="13"/>
          <line x1="16" y1="17" x2="8" y2="17"/>
        </svg>
      </div>
      <h4>Açıklama & Detaylar</h4>
    </div>

    <!-- View Mode: Rendered Rich Markdown -->
    <div v-if="!isEditing" class="description-view">
      <div
        v-if="description && description.trim()"
        class="description-markdown"
        v-html="renderedMarkdown"
      ></div>
      <div v-else class="description-empty">
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/>
          <line x1="12" y1="8" x2="12" y2="12"/>
          <line x1="12" y1="16" x2="12.01" y2="16"/>
        </svg>
        <span>Bu talep için açıklama belirtilmemiş.</span>
      </div>
    </div>

    <!-- Edit Mode: RichTextEditor with Markdown Toolbar & Preview -->
    <div v-else class="description-edit">
      <RichTextEditor
        :model-value="editedDescription"
        @update:model-value="$emit('update:editedDescription', $event)"
        placeholder="Talep açıklamasını düzenleyin... Markdown formatı desteklenir."
        :rows="9"
      />
    </div>
  </div>
</template>

<script>
import { marked } from 'marked'
import RichTextEditor from '../../common/RichTextEditor.vue'

marked.setOptions({
  gfm: true,
  breaks: true
})

export default {
  name: 'TicketModalDescription',
  components: {
    RichTextEditor
  },
  props: {
    description: {
      type: String,
      default: ''
    },
    isEditing: {
      type: Boolean,
      default: false
    },
    editedDescription: {
      type: String,
      default: ''
    }
  },
  emits: ['update:editedDescription'],
  computed: {
    renderedMarkdown() {
      if (!this.description) return ''
      try {
        return marked.parse(this.description)
      } catch (e) {
        return `<p>${this.description}</p>`
      }
    }
  }
}
</script>

<style scoped>
.ticket-description-box {
  display: flex;
  flex-direction: column;
  gap: 12px;
  height: 100%;
}

.description-header {
  display: flex;
  align-items: center;
  gap: 8px;
}

.desc-icon {
  color: var(--primary-color, #2563EB);
  display: flex;
}

.description-header h4 {
  font-size: 13px;
  font-weight: 700;
  color: var(--text-main, #1E293B);
  margin: 0;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.description-view {
  background: var(--bg-muted, #F8FAFC);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 12px;
  padding: 18px 20px;
  flex: 1;
  min-height: 220px;
  max-height: 480px;
  overflow-y: auto;
  transition: background-color 0.2s ease, border-color 0.2s ease;
}

.description-empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 180px;
  color: var(--text-muted, #94A3B8);
  gap: 8px;
  font-size: 13px;
}

/* Markdown styling inside view */
.description-markdown {
  font-size: 14px;
  line-height: 1.7;
  color: var(--text-main, #334155);
  word-break: break-word;
}

.description-markdown deep(h1),
.description-markdown deep(h2),
.description-markdown deep(h3) {
  color: var(--text-main, #0F172A);
  margin-top: 14px;
  margin-bottom: 8px;
  font-weight: 700;
}

.description-markdown deep(h2) {
  font-size: 17px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  padding-bottom: 4px;
}

.description-markdown deep(h3) {
  font-size: 15px;
}

.description-markdown deep(p) {
  margin-bottom: 10px;
}

.description-markdown deep(ul),
.description-markdown deep(ol) {
  padding-left: 22px;
  margin-bottom: 10px;
}

.description-markdown deep(li) {
  margin-bottom: 4px;
}

.description-markdown deep(code) {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  color: var(--primary-color, #2563EB);
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
  font-family: 'Courier New', Courier, monospace;
}

.description-markdown deep(pre) {
  background: var(--bg-surface, #0F172A);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 12px 16px;
  border-radius: 8px;
  overflow-x: auto;
  margin-bottom: 12px;
}

.description-markdown deep(pre code) {
  background: transparent;
  border: none;
  padding: 0;
  color: #38BDF8;
}

.description-markdown deep(blockquote) {
  border-left: 4px solid var(--primary-color, #2563EB);
  padding: 6px 14px;
  margin: 10px 0;
  color: var(--text-secondary, #475569);
  background: var(--bg-surface, #FFFFFF);
  border-radius: 0 8px 8px 0;
  font-style: italic;
}

.description-edit {
  flex: 1;
}
</style>

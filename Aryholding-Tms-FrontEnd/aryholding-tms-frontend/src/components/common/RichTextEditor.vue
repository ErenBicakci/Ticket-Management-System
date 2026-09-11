<template>
  <div class="rich-text-editor" :class="{ 'is-focused': isFocused }">
    <!-- Header with Toolbar & Mode Switcher -->
    <div class="editor-header">
      <!-- Toolbar (Visible in write mode) -->
      <div v-if="activeTab === 'write'" class="toolbar">
        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('bold')"
          title="Kalın (Ctrl+B)"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <path d="M6 4h8a4 4 0 0 1 4 4 4 4 0 0 1-4 4H6z"/>
            <path d="M6 12h9a4 4 0 0 1 4 4 4 4 0 0 1-4 4H6z"/>
          </svg>
        </button>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('italic')"
          title="İtalik (Ctrl+I)"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="19" y1="4" x2="10" y2="4"/>
            <line x1="14" y1="20" x2="5" y2="20"/>
            <line x1="15" y1="4" x2="9" y2="20"/>
          </svg>
        </button>

        <div class="tool-divider"></div>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('h2')"
          title="Başlık 2"
        >
          <span class="tool-text">H2</span>
        </button>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('h3')"
          title="Başlık 3"
        >
          <span class="tool-text">H3</span>
        </button>

        <div class="tool-divider"></div>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('bullet-list')"
          title="Madde İşaretli Liste"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <line x1="8" y1="6" x2="21" y2="6"/>
            <line x1="8" y1="12" x2="21" y2="12"/>
            <line x1="8" y1="18" x2="21" y2="18"/>
            <circle cx="3" cy="6" r="1.5" fill="currentColor"/>
            <circle cx="3" cy="12" r="1.5" fill="currentColor"/>
            <circle cx="3" cy="18" r="1.5" fill="currentColor"/>
          </svg>
        </button>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('numbered-list')"
          title="Numaralı Liste"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <line x1="10" y1="6" x2="21" y2="6"/>
            <line x1="10" y1="12" x2="21" y2="12"/>
            <line x1="10" y1="18" x2="21" y2="18"/>
            <path d="M4 6h2M5 6v3M4 14h2l-2 3h2"/>
          </svg>
        </button>

        <div class="tool-divider"></div>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('code')"
          title="Kod Bloğu"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="16 18 22 12 16 6"/>
            <polyline points="8 6 2 12 8 18"/>
          </svg>
        </button>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('quote')"
          title="Alıntı"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M3 21c3 0 7-1 7-8V5c0-1.25-.75-2-2-2H4c-1.25 0-2 .75-2 2v6c0 7 1 8 3 10z"/>
            <path d="M15 21c3 0 7-1 7-8V5c0-1.25-.75-2-2-2h-4c-1.25 0-2 .75-2 2v6c0 7 1 8 3 10z"/>
          </svg>
        </button>

        <button
          type="button"
          class="tool-btn"
          @click="insertFormat('link')"
          title="Bağlantı Ekle"
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M10 13a5 5 0 0 0 7.54.54l3-3a5 5 0 0 0-7.07-7.07l-1.72 1.71"/>
            <path d="M14 11a5 5 0 0 0-7.54-.54l-3 3a5 5 0 0 0 7.07 7.07l1.71-1.71"/>
          </svg>
        </button>
      </div>
      <div v-else class="toolbar-placeholder">
        <span class="preview-mode-label">Markdown Önizleme Modu</span>
      </div>

      <!-- Mode Tabs (Write / Preview) -->
      <div class="mode-tabs">
        <button
          type="button"
          class="mode-btn"
          :class="{ active: activeTab === 'write' }"
          @click="activeTab = 'write'"
        >
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M12 20h9"/>
            <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"/>
          </svg>
          <span>Düzenle</span>
        </button>
        <button
          type="button"
          class="mode-btn"
          :class="{ active: activeTab === 'preview' }"
          @click="activeTab = 'preview'"
        >
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/>
            <circle cx="12" cy="12" r="3"/>
          </svg>
          <span>Önizleme</span>
        </button>
      </div>
    </div>

    <!-- Content Area -->
    <div class="editor-body">
      <!-- Textarea for Write Mode -->
      <textarea
        v-show="activeTab === 'write'"
        ref="textareaRef"
        :value="modelValue"
        :placeholder="placeholder"
        :rows="rows"
        class="editor-textarea"
        @input="handleInput"
        @focus="isFocused = true"
        @blur="isFocused = false"
        @keydown="handleKeyDown"
      ></textarea>

      <!-- Rendered HTML for Preview Mode -->
      <div
        v-show="activeTab === 'preview'"
        class="editor-preview markdown-content"
        :style="{ minHeight: `${rows * 24}px` }"
        v-html="renderedMarkdown"
      ></div>
    </div>

    <!-- Footer Stats -->
    <div class="editor-footer">
      <div class="markdown-hint">
        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/>
          <line x1="12" y1="16" x2="12" y2="12"/>
          <line x1="12" y1="8" x2="12.01" y2="8"/>
        </svg>
        <span>Markdown desteklenir</span>
      </div>
      <div class="word-counter">
        {{ charCount }} karakter · {{ wordCount }} kelime
      </div>
    </div>
  </div>
</template>

<script>
import { marked } from 'marked'

// Configure marked
marked.setOptions({
  gfm: true,
  breaks: true
})

export default {
  name: 'RichTextEditor',
  props: {
    modelValue: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: 'Açıklama giriniz... Detaylı bilgi, hata adımları veya markdown formatında yazabilirsiniz.'
    },
    rows: {
      type: Number,
      default: 6
    }
  },
  emits: ['update:modelValue'],
  data() {
    return {
      activeTab: 'write',
      isFocused: false
    }
  },
  computed: {
    renderedMarkdown() {
      if (!this.modelValue || !this.modelValue.trim()) {
        return '<p class="empty-preview">Önizlenecek bir içerik henüz girilmedi.</p>'
      }
      try {
        return marked.parse(this.modelValue)
      } catch (e) {
        return `<p>${this.modelValue}</p>`
      }
    },
    charCount() {
      return (this.modelValue || '').length
    },
    wordCount() {
      if (!this.modelValue || !this.modelValue.trim()) return 0
      return this.modelValue.trim().split(/\s+/).length
    }
  },
  methods: {
    handleInput(event) {
      this.$emit('update:modelValue', event.target.value)
    },
    handleKeyDown(event) {
      if ((event.ctrlKey || event.metaKey) && event.key === 'b') {
        event.preventDefault()
        this.insertFormat('bold')
      } else if ((event.ctrlKey || event.metaKey) && event.key === 'i') {
        event.preventDefault()
        this.insertFormat('italic')
      }
    },
    insertFormat(type) {
      const textarea = this.$refs.textareaRef
      if (!textarea) return

      const start = textarea.selectionStart
      const end = textarea.selectionEnd
      const text = textarea.value
      const selected = text.substring(start, end)

      let replacement = ''
      let newCursorPos = start

      switch (type) {
        case 'bold':
          replacement = selected ? `**${selected}**` : '**kalın metin**'
          newCursorPos = selected ? end + 4 : start + 2
          break
        case 'italic':
          replacement = selected ? `*${selected}*` : '*italik metin*'
          newCursorPos = selected ? end + 2 : start + 1
          break
        case 'h2':
          replacement = selected ? `\n## ${selected}\n` : '\n## Başlık\n'
          newCursorPos = selected ? end + 5 : start + 4
          break
        case 'h3':
          replacement = selected ? `\n### ${selected}\n` : '\n### Alt Başlık\n'
          newCursorPos = selected ? end + 6 : start + 5
          break
        case 'bullet-list':
          if (selected) {
            const lines = selected.split('\n').map(l => `- ${l}`).join('\n')
            replacement = `\n${lines}\n`
          } else {
            replacement = '\n- Liste elemanı\n- İkinci eleman\n'
          }
          newCursorPos = start + replacement.length
          break
        case 'numbered-list':
          if (selected) {
            const lines = selected.split('\n').map((l, idx) => `${idx + 1}. ${l}`).join('\n')
            replacement = `\n${lines}\n`
          } else {
            replacement = '\n1. Birinci eleman\n2. İkinci eleman\n'
          }
          newCursorPos = start + replacement.length
          break
        case 'code':
          if (selected && selected.includes('\n')) {
            replacement = `\n\`\`\`\n${selected}\n\`\`\`\n`
          } else if (selected) {
            replacement = `\`${selected}\``
          } else {
            replacement = '\n```\n// kod buraya\n```\n'
          }
          newCursorPos = start + replacement.length
          break
        case 'quote':
          replacement = selected ? `\n> ${selected}\n` : '\n> Alıntı metni\n'
          newCursorPos = start + replacement.length
          break
        case 'link':
          replacement = selected ? `[${selected}](https://)` : '[Bağlantı metni](https://)'
          newCursorPos = start + replacement.length - 1
          break
        default:
          return
      }

      const updatedText = text.substring(0, start) + replacement + text.substring(end)
      this.$emit('update:modelValue', updatedText)

      this.$nextTick(() => {
        textarea.focus()
        textarea.setSelectionRange(newCursorPos, newCursorPos)
      })
    }
  }
}
</script>

<style scoped>
.rich-text-editor {
  border: 1px solid var(--border-color, #CBD5E1);
  border-radius: 10px;
  background: var(--bg-surface, #FFFFFF);
  overflow: hidden;
  transition: all 0.2s ease;
  display: flex;
  flex-direction: column;
}

.rich-text-editor.is-focused {
  border-color: var(--primary-color, #2563EB);
  box-shadow: 0 0 0 3px var(--primary-light, rgba(37, 99, 235, 0.15));
}

/* Header & Toolbar */
.editor-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 12px;
  background: var(--bg-muted, #F8FAFC);
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  gap: 12px;
  flex-wrap: wrap;
}

.toolbar {
  display: flex;
  align-items: center;
  gap: 4px;
  flex-wrap: wrap;
}

.tool-btn {
  background: transparent;
  border: 1px solid transparent;
  color: var(--text-secondary, #475569);
  padding: 6px 8px;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 600;
  transition: all 0.15s ease;
}

.tool-btn:hover {
  background: var(--bg-surface, #FFFFFF);
  color: var(--primary-color, #2563EB);
  border-color: var(--border-color, #CBD5E1);
}

.tool-text {
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.5px;
}

.tool-divider {
  width: 1px;
  height: 18px;
  background: var(--border-color, #E2E8F0);
  margin: 0 4px;
}

.toolbar-placeholder {
  display: flex;
  align-items: center;
  padding-left: 6px;
}

.preview-mode-label {
  font-size: 12px;
  font-weight: 600;
  color: var(--primary-color, #2563EB);
}

/* Mode Switcher Tabs */
.mode-tabs {
  display: flex;
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 7px;
  padding: 2px;
  gap: 2px;
  margin-left: auto;
}

.mode-btn {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 5px 10px;
  border-radius: 5px;
  border: none;
  background: transparent;
  color: var(--text-muted, #64748B);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s ease;
}

.mode-btn.active {
  background: var(--primary-color, #2563EB);
  color: #FFFFFF;
  box-shadow: 0 1px 3px rgba(37, 99, 235, 0.3);
}

/* Editor Body */
.editor-body {
  position: relative;
  width: 100%;
}

.editor-textarea {
  width: 100%;
  border: none;
  outline: none;
  padding: 14px 16px;
  background: var(--bg-surface, #FFFFFF);
  color: var(--text-main, #0F172A);
  font-family: inherit;
  font-size: 14px;
  line-height: 1.6;
  resize: vertical;
  min-height: 140px;
}

.editor-textarea::placeholder {
  color: var(--input-placeholder, #94A3B8);
}

.editor-preview {
  padding: 14px 16px;
  background: var(--bg-surface, #FFFFFF);
  color: var(--text-main, #0F172A);
  font-size: 14px;
  line-height: 1.6;
  overflow-y: auto;
}

.empty-preview {
  color: var(--text-muted, #94A3B8);
  font-style: italic;
  margin: 0;
}

/* Markdown formatting inside preview */
.markdown-content deep(h1),
.markdown-content deep(h2),
.markdown-content deep(h3) {
  color: var(--text-main, #0F172A);
  margin-top: 12px;
  margin-bottom: 8px;
  font-weight: 700;
}

.markdown-content deep(h2) {
  font-size: 18px;
  border-bottom: 1px solid var(--border-color, #E2E8F0);
  padding-bottom: 4px;
}

.markdown-content deep(h3) {
  font-size: 15px;
}

.markdown-content deep(p) {
  margin-bottom: 8px;
}

.markdown-content deep(ul),
.markdown-content deep(ol) {
  padding-left: 24px;
  margin-bottom: 8px;
}

.markdown-content deep(li) {
  margin-bottom: 4px;
}

.markdown-content deep(code) {
  background: var(--bg-muted, #F1F5F9);
  color: var(--primary-color, #2563EB);
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
  font-family: 'Courier New', Courier, monospace;
}

.markdown-content deep(pre) {
  background: var(--bg-muted, #F1F5F9);
  border: 1px solid var(--border-color, #E2E8F0);
  padding: 12px;
  border-radius: 8px;
  overflow-x: auto;
  margin-bottom: 8px;
}

.markdown-content deep(pre code) {
  background: transparent;
  padding: 0;
  color: var(--text-main, #0F172A);
}

.markdown-content deep(blockquote) {
  border-left: 4px solid var(--primary-color, #2563EB);
  padding-left: 12px;
  margin: 8px 0;
  color: var(--text-secondary, #475569);
  font-style: italic;
  background: var(--bg-muted, #F8FAFC);
  padding-top: 4px;
  padding-bottom: 4px;
  border-radius: 0 6px 6px 0;
}

/* Footer */
.editor-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 6px 14px;
  background: var(--bg-muted, #F8FAFC);
  border-top: 1px solid var(--border-subtle, #F1F5F9);
  font-size: 11px;
  color: var(--text-muted, #94A3B8);
}

.markdown-hint {
  display: flex;
  align-items: center;
  gap: 6px;
}

.word-counter {
  font-weight: 500;
}
</style>

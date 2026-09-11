<template>
  <div class="skeleton-wrapper" :class="[`type-${type}`]">
    <!-- Single / Multiple Lines Text -->
    <template v-if="type === 'text'">
      <div
        v-for="i in count"
        :key="i"
        class="skeleton-item skeleton-text"
        :style="{
          width: width ? (typeof width === 'number' ? width + 'px' : width) : (i === count && count > 1 ? '70%' : '100%'),
          height: height ? (typeof height === 'number' ? height + 'px' : height) : '14px'
        }"
      ></div>
    </template>

    <!-- Avatar Skeleton -->
    <template v-else-if="type === 'avatar'">
      <div
        class="skeleton-item skeleton-avatar"
        :style="{
          width: width ? (typeof width === 'number' ? width + 'px' : width) : '40px',
          height: height ? (typeof height === 'number' ? height + 'px' : height) : '40px'
        }"
      ></div>
    </template>

    <!-- Card Skeleton -->
    <template v-else-if="type === 'card'">
      <div v-for="i in count" :key="i" class="skeleton-card">
        <div class="skeleton-card-header">
          <div class="skeleton-item skeleton-badge"></div>
          <div class="skeleton-item skeleton-badge-sm"></div>
        </div>
        <div class="skeleton-item skeleton-title"></div>
        <div class="skeleton-item skeleton-desc"></div>
        <div class="skeleton-card-footer">
          <div class="skeleton-item skeleton-avatar-sm"></div>
          <div class="skeleton-item skeleton-date"></div>
        </div>
      </div>
    </template>

    <!-- Table Rows Skeleton -->
    <template v-else-if="type === 'table'">
      <div v-for="i in count" :key="i" class="skeleton-table-row">
        <div class="skeleton-item col-id"></div>
        <div class="skeleton-item col-title"></div>
        <div class="skeleton-item col-dept"></div>
        <div class="skeleton-item col-status"></div>
        <div class="skeleton-item col-date"></div>
      </div>
    </template>

    <!-- Generic Rectangle / Box Skeleton -->
    <template v-else>
      <div
        v-for="i in count"
        :key="i"
        class="skeleton-item skeleton-rect"
        :style="{
          width: width ? (typeof width === 'number' ? width + 'px' : width) : '100%',
          height: height ? (typeof height === 'number' ? height + 'px' : height) : '100px'
        }"
      ></div>
    </template>
  </div>
</template>

<script>
export default {
  name: 'SkeletonLoader',
  props: {
    type: {
      type: String,
      default: 'card', // 'card', 'text', 'avatar', 'table', 'rect'
      validator: val => ['card', 'text', 'avatar', 'table', 'rect'].includes(val)
    },
    count: {
      type: Number,
      default: 1
    },
    width: {
      type: [String, Number],
      default: null
    },
    height: {
      type: [String, Number],
      default: null
    }
  }
}
</script>

<style scoped>
.skeleton-wrapper {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.skeleton-item {
  background: var(--skeleton-bg, #E2E8F0);
  border-radius: 6px;
  position: relative;
  overflow: hidden;
}

.skeleton-item::after {
  content: "";
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  transform: translateX(-100%);
  background: linear-gradient(
    90deg,
    rgba(255, 255, 255, 0) 0%,
    var(--skeleton-shimmer, rgba(255, 255, 255, 0.6)) 50%,
    rgba(255, 255, 255, 0) 100%
  );
  animation: shimmer 1.6s infinite ease-in-out;
}

@keyframes shimmer {
  100% {
    transform: translateX(100%);
  }
}

.skeleton-avatar {
  border-radius: 50%;
}

.skeleton-text {
  margin-bottom: 6px;
}

/* Card Skeleton */
.skeleton-card {
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 12px;
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  box-shadow: var(--shadow-sm);
}

.skeleton-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.skeleton-badge {
  width: 70px;
  height: 22px;
  border-radius: 20px;
}

.skeleton-badge-sm {
  width: 45px;
  height: 20px;
  border-radius: 4px;
}

.skeleton-title {
  width: 85%;
  height: 18px;
}

.skeleton-desc {
  width: 60%;
  height: 13px;
}

.skeleton-card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 8px;
  border-top: 1px solid var(--border-subtle, #F1F5F9);
}

.skeleton-avatar-sm {
  width: 24px;
  height: 24px;
  border-radius: 50%;
}

.skeleton-date {
  width: 80px;
  height: 12px;
}

/* Table Skeleton */
.skeleton-table-row {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 14px 16px;
  background: var(--bg-surface, #FFFFFF);
  border: 1px solid var(--border-color, #E2E8F0);
  border-radius: 8px;
}

.col-id { width: 50px; height: 16px; }
.col-title { flex: 2; height: 16px; }
.col-dept { flex: 1; height: 16px; }
.col-status { width: 90px; height: 24px; border-radius: 20px; }
.col-date { width: 80px; height: 14px; }
</style>

<template>
  <div id="app">
    <ToastContainer />

    <div v-if="isAuthRoute" class="auth-layout-wrapper">
      <router-view />
    </div>

    <div v-else class="main-layout-wrapper">
      <AppLayout>
        <router-view />
      </AppLayout>
    </div>
  </div>
</template>

<script>
import ToastContainer from './components/common/ToastContainer.vue'
import AppLayout from './components/Layout.vue'

export default {
  name: 'App',
  components: {
    ToastContainer,
    AppLayout
  },
  computed: {
    isAuthRoute() {
      const path = this.$route ? this.$route.path : ''
      return path === '/login' || path === '/signup'
    }
  }
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Inter', 'Segoe UI', -apple-system, BlinkMacSystemFont, sans-serif;
  background-color: #F1F5F9;
  color: #0F172A;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  width: 100%;
  min-height: 100vh;
}

.auth-layout-wrapper {
  width: 100%;
  min-height: 100vh;
}

.main-layout-wrapper {
  width: 100%;
  min-height: 100vh;
}
</style>

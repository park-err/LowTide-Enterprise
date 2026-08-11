import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import App from './App.vue'
import { router } from './router'
import { useAuthStore } from './stores/auth'
import vue3GoogleLogin from 'vue3-google-login'
import process from 'node:process'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(vue3GoogleLogin, {
  clientId: process.env.GOOGLE_CLIENT_ID,
})

const authStore = useAuthStore()
authStore.initializeFromCookie().finally(() => {
  app.mount('#app')
})

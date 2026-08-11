<template>
  <div class="flex flex-col justify-center items-center gap-5 w-screen mx-auto mt-24 p-6">
    <div class="mb-10">
      <!-- Logo -->
      <img style="width: 300px" src="../../public/logo.png" />
    </div>
    <section class="flex flex-col justify-center items-center bg-white w-[30svw] min-w-100 h-70 rounded-4xl">
      <h1 class="text-3xl mb-12">Sign in</h1>
      <GoogleLogin uxMode="popup" :callback="handleSubmit" />
      <p v-show="authStore.hasError" class="mt-6 text-red-600">{{authStore.error}}</p>
    </section>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { useAuthStore } from '@/stores/auth'
  import { useInitStore } from '@/stores/init'
  import { GoogleLogin } from 'vue3-google-login'
  import type { CredentialPopupResponse } from '@/types/auth'

  const submitting = ref(false)

  const authStore = useAuthStore()
  const initStore = useInitStore()
  const router = useRouter()
  const route = useRoute()

  async function handleSubmit(response: CredentialPopupResponse): Promise<void> {
    submitting.value = true
    try {
      await authStore.authenticate(response)
      await authStore.setCurrentUser()
      const redirect = route.query.redirect
      router.push(typeof redirect === 'string' ? redirect : { name: 'home' })
    } catch {
      // authStore.error already holds a user-facing message; nothing else to do here.
    } finally {
      await initStore.initApp()
      submitting.value = false
    }
  }
</script>

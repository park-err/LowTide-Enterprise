<script setup lang="ts">
import Header from '@/components/Header.vue'
import Footer from '@/components/Footer.vue'
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="min-h-screen flex flex-col bg-(--color-bl-page)">
    <Header />
    <RouterView class="flex-1" />
    <Footer />
  </div>
</template>

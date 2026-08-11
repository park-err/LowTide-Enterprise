<template>
  <section>
    <h2 class="text-bl-blue font-bold mb-4">LowTide Staff Links</h2>
    <div class="grid grid-cols-2 sm:grid-cols-4 gap-3">
      <a v-for="link in staffLinks"
         :key="link"
         :href="link.linkUrl"
         class="bg-white rounded-md border border-slate-200 shadow-sm text-center text-sm font-medium text-slate-700 px-3 py-4 hover:border-bl-blue hover:text-bl-blue transition-colors">
        {{ link.title }}
      </a>
    </div>
  </section>
</template>

<script setup lang="ts">
  import type { StaffLink } from '@/types/types'

  defineProps<{
    staffLinks: StaffLink[]
  }>()
</script>

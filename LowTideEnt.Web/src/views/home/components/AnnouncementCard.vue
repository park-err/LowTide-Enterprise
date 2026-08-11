<template>
  <article class="bg-white rounded-lg overflow-hidden shadow-sm border border-slate-100">
    <div class="h-32 bg-slate-300"></div>
    <div class="p-4">
      <h3 class="font-semibold text-slate-800 text-sm">{{ title }}</h3>
      <p class="text-xs text-slate-400 mt-0.5">{{ date }}</p>
      <p class="text-xs text-slate-500 mt-2 leading-relaxed">{{ body }}</p>
    </div>
  </article>
</template>

<script setup lang="ts">
  defineProps<{
    title: string
    date: string
    body: string
  }>()
</script>

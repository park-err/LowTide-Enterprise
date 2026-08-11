<template>
  <section v-if="shiningStar" class="bg-white rounded-lg shadow-sm border border-slate-100 p-5 h-90">
    <h2 class="flex items-center gap-2 text-slate-700 font-semibold mb-4">
      <svg class="w-5 h-5 text-amber-400" fill="currentColor" viewBox="0 0 20 20">
        <path
          d="M10 1.5l2.6 5.8 6.4.6-4.8 4.2 1.5 6.3L10 15.2l-5.7 3.2 1.5-6.3-4.8-4.2 6.4-.6L10 1.5z"
        />
      </svg>
      Shining Stars
    </h2>

    <div
      class="rounded-md overflow-hidden bg-gradient-to-r from-[#AF9F6B]/50 to-[#70829B]/50 p-8 text-center"
    >
      <div
        class="w-14 h-14 mx-auto rounded-full bg-amber-400 flex items-center justify-center shadow"
      >
        <svg class="w-7 h-7 text-white" fill="currentColor" viewBox="0 0 20 20">
          <path
            d="M10 1.5l2.6 5.8 6.4.6-4.8 4.2 1.5 6.3L10 15.2l-5.7 3.2 1.5-6.3-4.8-4.2 6.4-.6L10 1.5z"
          />
        </svg>
      </div>

      <p class="mt-3 font-bold text-bl-navy text-base">Low Tide Shining Star!</p>
      <p class="text-xs text-slate-600 mt-1">{{ shiningStar.nominationDate }}</p>
      <p class="mt-3 font-semibold text-slate-800">{{ shiningStar.fullName }}</p>
      <p class="mt-1 text-sm italic text-slate-600">&ldquo;{{ shiningStar.quote }}&rdquo;</p>
      <p class="mt-3 text-xs text-slate-500">{{ shiningStar.value }}</p>
    </div>
  </section>
  <section v-else class="bg-white/50 rounded-lg shadow-sm border border-slate-100 p-5 h-90"></section>
</template>

<script setup lang="ts">
  import type { ShiningStar } from '@/types/types'
  defineProps<{
    shiningStar: ShiningStar | null
  }>()
</script>

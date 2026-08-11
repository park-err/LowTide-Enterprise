<script setup lang="ts">
  import { computed } from 'vue'
  interface Props {
    color?: string
    size?: number | string
  }

  const props = withDefaults(defineProps<Props>(), {
    color: 'currentColor',
    size: 24,
  })

  const width = props.size
  const height = computed(() => props.size / 1.5)
 </script>

<template>
  <svg :width="width"
       :height="height"
       viewBox="-1 0 50 28"
       fill="none"
       xmlns="http://www.w3.org/2000/svg">
    <rect width="47" height="4" rx="2" :fill="color" />
    <rect y="12" width="47" height="4" rx="2" :fill="color" />
    <rect y="24" width="47" height="4" rx="2" :fill="color" />
  </svg>
</template>

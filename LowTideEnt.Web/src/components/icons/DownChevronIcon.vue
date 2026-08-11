<script setup lang="ts">import { computed } from 'vue'
  interface Props {
    color?: string
    size?: number | string
  }

  const props = withDefaults(defineProps<Props>(), {
    color: 'currentColor',
    size: 24,
  })

 </script>

<template>
  <svg :width="size"
       :height="size"
       viewBox="0 0 50 50"
       fill="none"
       xmlns="http://www.w3.org/2000/svg">
    <rect x="12.5664" y="14.8438" width="20.9927" height="3.1" transform="rotate(-45 12.5664 14.8438)" fill="color" />
    <rect x="2.1897" y="0.0878906" width="20.8675" height="3.09678" transform="rotate(45 2.1897 0.0878906)" fill="color" />
  </svg>
</template>

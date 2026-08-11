<script setup lang="ts">
interface Props {
  color?: string
  size?: number | string
}

withDefaults(defineProps<Props>(), {
  color: 'currentColor',
  size: 24,
})
</script>

<template>
  <svg
    :width="size"
    :height="size"
    viewBox="-10 -10 50 50"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
  >
    <rect
      x="1"
      y="1"
      width="22.4034"
      height="22.4039"
      rx="11.2017"
      :stroke="color"
      stroke-width="2"
    />
    <path
      d="M19.5164 21.8184L21.7314 19.6032L26.8925 24.7644C27.5042 25.3761 27.5042 26.3678 26.8925 26.9795C26.2808 27.5912 25.2891 27.5912 24.6774 26.9795L19.5164 21.8184Z"
      :fill="color"
    />
  </svg>
</template>

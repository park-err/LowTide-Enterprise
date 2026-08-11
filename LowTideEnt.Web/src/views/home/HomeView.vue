<template>
  <div class="flex flex-col justify-center items-center gap-10 m-10">
    <div>
      <h1 class="text-3xl font-bold text-center text-bl-blue">Low Tide Enterprise</h1>
    </div>
    <ShiningStars
        :shiningStar="homeStore.shiningStar"
        class="w-125 justify-self-center"
      />
    <Announcement :announcements="homeStore.announcements" class="w-300 justify-self-center" />
    <StaffLinks :staffLinks="homeStore.staffLinks" class="w-300 justify-self-center" />
  </div>
</template>

<script setup lang="ts">
  import PermissionGate from '@/components/PermissionGate.vue'
  import Announcement from './components/Announcements.vue'
  import ShiningStars from './components/ShiningStars.vue'
  import StaffLinks from './components/StaffLinks.vue'
  import { useHomeStore } from '@/stores/homeStore'
  import { onMounted, ref } from 'vue'

  const homeStore = useHomeStore()
  const isLoading = ref(true)

  onMounted(async () => {
    await homeStore.initHomePage()
    isLoading.value = false
  })
  
</script>

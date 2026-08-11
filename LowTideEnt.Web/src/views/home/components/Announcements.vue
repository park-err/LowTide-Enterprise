<template>
  <section>
    <h2 class="text-bl-blue font-bold mb-4">Low Tide Announcements</h2>
    <div class="flex flex-row gap-4">
      <AnnouncementCard v-for="(announcement, i) in announcements"
        :key="i"
        :title="announcement.title"
        :date="announcement.postedDate"
        :body="announcement.body"
      />
    </div>
  </section>
</template>

<script setup lang="ts">
  import AnnouncementCard from './AnnouncementCard.vue'
  import type { Announcement } from '../../../types/types.ts'
  import { computed } from 'vue'

  const props = defineProps<{
    announcements: Announcement[]
  }>()
  const topAnnouncements = computed(() => props.announcements.slice(0, 3));

</script>

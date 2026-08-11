<template>
  <div class="flex flex-row gap-10">
    <ResourceLibraryMenu :resourceList="resourceStore.resourceList" @fetch-resource="fetchResourceById" />
    <div v-if="resourceStore.resourceHtml" v-html="resourceStore.resourceHtml"></div>
  </div>
</template>

<script setup lang="ts">
  import { useResourceStore } from './store/resourceStore'
  import { onMounted, watch } from 'vue'
  import { useRoute, onBeforeRouteUpdate } from 'vue-router'
  import ResourceLibraryMenu from './components/ResourceLibraryMenuComponent.vue'

  const route = useRoute()
  const resourceStore = useResourceStore()

  function parseId(raw: unknown): number {
    const value = Array.isArray(raw) ? raw[0] : raw
    const parsed = Number(value)
    return Number.isNaN(parsed) ? 0 : parsed
  }

  async function loadResources(categoryId: number) {
    await resourceStore.fetchResources(categoryId)
  }

  onMounted(() => {
    loadResources(parseId(route.query.categoryId))
  })

  watch(
    () => route.query.type,
    (newType) => {
      loadResources(parseId(newType))
    },
    { immediate: true }
  )

  const fetchResourceById = () => {
    resourceStore.fetchResourceContent(22, 2);
  }
</script>

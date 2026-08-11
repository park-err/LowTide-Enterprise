<template>
  <div class="flex flex-col gap-10 w-100 bg-white transition-transform duration-100 ease-in-out" :class="showMenu ? '-translate-x-0' : '-translate-x-80'">
    <button @click="showMenu = !showMenu" class="w-10 h-10 ml-auto mt-5 mr-5 rounded-full hover:cursor-pointer">
      <MenuIcon color="var(--color-bl-grey)" class="m-auto h-[100%]" />
    </button>
    <ul>
      <li v-for="(cat, i) in resourceList.childList" :key="i" class="bg-red-100 h-auto p-2">
        {{cat.categoryName}}
        <ul v-if="cat.resources">
          <li @click="$emit('fetch-resource')" v-for="resource in cat.resources" :key="resource.id">{{resource.title}}</li>
        </ul>
        <ul v-if="cat.childList">
          <li v-for="subCat in cat.childList" :key="subCat.categoryId">{{cat.categoryName}}</li>
        </ul>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import MenuIcon from '@/components/icons/MenuIcon.vue'
  import type { ResourceList } from '../store/types'
  import { computed, ref } from 'vue'

  defineProps<{
    resourceList: ResourceList
  }>();
  defineEmits(['fetch-resource'])

  const showMenu = ref(true);
</script>

<script setup lang="ts">
  import type { MenuItem } from '@/types/menu'
  import { RouterLink } from 'vue-router'
  import { computed } from 'vue'

  const props = defineProps<{
    subMenu: MenuItem[]
  }>()
</script>
<template>
  <div class="flex flex-row gap-5 w-150 h-100 bg-white border-2 border-[#3E64B3] ml-auto text-left p-5">
     <div v-for="(item, i) in subMenu" :key="i" class="mb-5">
       <h6 class="hover:cursor-default text-lg">{{item.title}}</h6>
       <ul>
         <!-- TODO: Can we make this recursive? -->
         <li v-for="(subItem, i) in item.subMenu" class="mb-5 mt-5 font-normal">
           <RouterLink :to="{ path: '/resource-library', query: { categoryId: subItem.id } }">
             {{ subItem.title }}
           </RouterLink>
         </li>
       </ul>
     </div>
  </div>
</template>

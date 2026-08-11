<template>
  <header v-show="showHeader" class="bg-bl-teal/50 px-[10vw] pt-5 pb-1">
    <div class="flex align-center justify-between m-auto">
      <div>
        <!-- Logo -->
        <img style="width: 150px" src="../../public/logo.png" />
      </div>
      <div class="my-auto">
        <!-- Search -->
        <label class="relative">
          <span class="sr-only">Search</span>
          <SearchIcon class="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400"
                      color="grey"
                      :size="20" />
          <input type="text"
                 placeholder="Search"
                 class="w-56 md:w-64 pl-9 pr-3 py-2 rounded-md text-sm bg-white/95 placeholder-slate-400 text-slate-700 focus:outline-none focus:ring-2 focus:ring-bl-navy" />
        </label>
      </div>
    </div>
    <nav>
      <ul class="flex gap-10 my-5 text-slate-800 font-semibold text-[15px] w-[100%]">
        <li class="last:ml-auto last:relative">
          <a class="hover:text-bl-navy transition-colors">Admin</a>
        </li>
        <li class="last:ml-auto last:relative">
          <a class="hover:text-bl-navy transition-colors">Front Desk</a>
        </li>
        <li class="last:ml-auto last:relative">
        <a>Billing</a>
          </li>
        <li class="last:ml-auto last:relative">
          <a @click="showResourceSub = !showResourceSub" class="hover:text-bl-navy hover:cursor-pointer transition-colors">
            Resource Library
          </a>
          <ResourceLibraryNav v-show="showResourceSub"
                              class="absolute top-full right-0 z-50"
                              :subMenu="resourceSubMenu" />
        </li>
      </ul>
    </nav>
  </header>
</template>

<script setup lang="ts">
  import type { MenuItem } from '@/types/menu'
  import SearchIcon from './icons/SearchIcon.vue'
  import ResourceLibraryNav from './Nav/NavSubMenu.vue'
  import { usePermissions } from '@/composables/usePermissions'
  import { computed, ref } from 'vue'
  import { useInitStore } from '@/stores/init'
  import { onBeforeRouteUpdate } from 'vue-router'
  
  const { isAuthenticated } = usePermissions()
  const showResourceSub = ref(false);
  const initStore = useInitStore();
  const showHeader = computed(() => Boolean(isAuthenticated && initStore.appInitialized));
  const resourceSubMenu = computed(() => initStore.navMenu?.resourceMenu);
</script>

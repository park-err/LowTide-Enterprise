import { defineStore } from 'pinia'
import { initService } from '@/services/initService'
import type { Menu } from '@/types/menu'

interface AppState {
  navMenu: Menu | null
}

export const useInitStore = defineStore('init', {
  state: (): AppState => ({
    navMenu: null
  }),
  getters: {
    appInitialized: (state): boolean => Boolean(state.navMenu)
  },
  actions: {
    async initApp() {
      this.navMenu = await initService.fetchAppInit();
    }
  }

})

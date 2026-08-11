import { defineStore } from 'pinia'
import { homeService } from '../services/homeService'
import { toDate } from '@/helpers/dateHelper'
import type { ShiningStar, Announcement, StaffLink } from '../types/types'

interface HomeState {
  shiningStar: ShiningStar | null
  announcements: Announcement[]
  staffLinks: StaffLink[]
}

export const useHomeStore = defineStore('home', {
  state: (): HomeState => ({
    shiningStar: null,
    announcements: [],
    staffLinks: []
  }),
  getters: {
  },
  actions: {
    async initHomePage(): Promise<void> {
      const { shiningStar, announcements, staffLinks } = await homeService.fetchHomeComponents();
      announcements.map(a => a.postedDate = toDate(new Date(a.postedDate)))
      shiningStar.nominationDate = toDate(new Date(shiningStar.nominationDate))
      this.shiningStar = shiningStar
      this.announcements = announcements.splice(0, 3)
      this.staffLinks = staffLinks
    }
  }
})

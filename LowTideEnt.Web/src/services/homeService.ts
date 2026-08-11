import { httpClient } from '@/services/httpClient'
import type { ShiningStar, Announcement, StaffLink } from '../types/types'

interface HomeInit {
  shiningStar: ShiningStar
  announcements: Announcement[]
  staffLinks: StaffLink[]
}

export const homeService = {
  async fetchHomeComponents(): Promise<{ shiningStar: ShiningStar; announcements: Announcement[]; staffLinks: StaffLink[]; } > {
    const { data } = await httpClient.get<HomeInit>('/home')
    return {
      shiningStar: data.shiningStar,
      announcements: data.announcements,
      staffLinks: data.staffLinks
    } 
  }
}

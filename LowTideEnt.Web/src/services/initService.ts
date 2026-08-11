import { httpClient } from './httpClient'
import type { Menu } from '@/types/menu'

export const initService = {
  async fetchAppInit(): Promise<Menu> {
    const { data } = await httpClient.get<Menu>('/home/menu')
    return data
  }
}

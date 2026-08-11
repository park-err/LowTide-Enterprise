import { httpClient } from './httpClient'
import type { AuthUser } from '@/types/auth'

export const authService = {
  async authenticate(credentials: string): Promise<void> {
    await httpClient.post<void>('/auth', { credentials })
  },

  async fetchCurrentUser(): Promise<AuthUser> {
    const { data } = await httpClient.get<AuthUser>('/auth')
    return data
  },
}

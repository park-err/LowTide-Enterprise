import { defineStore } from 'pinia'
import { authService } from '../services/authService'
import { configureHttpClientAuth } from '../services/httpClient'
import { useHomeStore } from './homeStore'
import type { AuthStatus, AuthUser, CredentialPopupResponse } from '../types/auth'

interface AuthState {
  user: AuthUser | null
  status: AuthStatus
  error: string | null
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    user: null,
    status: 'idle',
    error: null,
  }),

  getters: {
    isAuthenticated: (state): boolean => Boolean(state.user && state.status === 'session_created'),
    currentUser: (state): AuthUser | null => state.user,
    hasError: (state): boolean => Boolean(state.error && state.status === 'error')
  },

  actions: {
    async authenticate(credentials: CredentialPopupResponse): Promise<void> {
      this.status = 'loading'
      this.error = null
      try {
        await authService.authenticate(credentials.credential)
        this.status = 'auth_ready'
      } catch (err) {
        console.log(err)
        this.error = extractErrorMessage(err)
        this.status = 'error'
        throw err
      }
    },

    async setCurrentUser(): Promise<void> {
      this.error = null
      try {
        const currentUser = await authService.fetchCurrentUser()
        this.user = currentUser
        this.status = 'session_created'
      } catch (err) {
        this.error = extractErrorMessage(err)
        this.status = 'error'
        throw err
      }
    },

    async initializeFromCookie(): Promise<void> {
      this.status = 'loading'
      try {
        const currentUser = await authService.fetchCurrentUser()
        this.user = currentUser
        this.status = 'session_created'
      } catch (err) {
        this.status = 'idle'
      }
    },
  },
})

function extractErrorMessage(err: unknown): string {
  if (
    typeof err === 'object' &&
    err !== null &&
    'response' in err &&
    typeof (err as { response?: { data?: { message?: string } } }).response?.data?.message ===
      'string'
  ) {
    return (err as { response: { data: { message: string } } }).response.data.message
  }
  return 'Unable to sign in.'
}

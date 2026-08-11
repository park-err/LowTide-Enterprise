import { computed, type ComputedRef } from 'vue'
import { useAuthStore } from '../stores/auth'
import type { AuthUser } from '../types/auth'

interface UsePermissionsReturn {
  isAuthenticated: ComputedRef<boolean>
  currentUser: ComputedRef<AuthUser | null>
}

export function usePermissions(): UsePermissionsReturn {
  const authStore = useAuthStore()

  const isAuthenticated = computed(() => authStore.isAuthenticated)
  const currentUser = computed(() => authStore.currentUser)

  return { isAuthenticated, currentUser }
}

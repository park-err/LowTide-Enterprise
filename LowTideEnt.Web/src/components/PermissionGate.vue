<script setup lang="ts">
import { computed } from 'vue'
import { usePermissions } from '@/composables/usePermissions'
import type { PermissionCheckMode } from '../types/auth'

// Usage:
//   <PermissionGate permission="billing:view">
//     <BillingWidget />
//   </PermissionGate>
//
//   <PermissionGate :permissions="['admin:access', 'billing:manage']" mode="any">
//     <AdminOrBillingContent />
//   </PermissionGate>
//
//   <PermissionGate role="admin">
//     <AdminBadge />
//   </PermissionGate>
const props = withDefaults(
  defineProps<{
    // permission?: string | null
    // permissions?: string[]
    // mode?: PermissionCheckMode // only used with `permissions`
    // role?: string | null
    isAuth: boolean
  }>(),
  {
    // permission: null,
    // permissions: () => [],
    // mode: 'all',
    // role: null,
    isAuth: false,
  },
)

const { isAuthenticated } = usePermissions()

const allowed = computed(() => {
  return isAuthenticated // TODO: set permissions
})
</script>

<template>
  <slot v-if="allowed" />
  <slot v-else name="fallback" />
</template>

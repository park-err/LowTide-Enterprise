import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Augment vue-router's RouteMeta so `meta.permission` / `meta.public` are
// typed and autocompleted on every route definition, instead of being `any`.
declare module 'vue-router' {
  interface RouteMeta {
    public?: boolean
    permission?: string
  }
}

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/LoginView.vue'),
    meta: { public: true },
  },
  {
    path: '/',
    name: 'home',
    component: () => import('@/views/home/HomeView.vue'),
    meta: {},
  },
  {
    path: '/resource-library',
    name: 'resource-library',
    component: () => import('@/views/resourceLibrary/ResourceLibraryView.vue'),
    meta: {},
  },
  // {
  //   path: '/admin',
  //   name: 'admin',
  //   component: () => import('../views/AdminView.vue'),
  //   meta: { permission: 'admin:access' },
  // },
  // {
  //   path: '/billing',
  //   name: 'billing',
  //   component: () => import('../views/BillingView.vue'),
  //   meta: { permission: 'billing:view' },
  // },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()

  if (to.meta.public) return true

  // Session hasn't been resolved yet (e.g. hard page refresh) — wait for it.
  if (authStore.status === 'idle' || authStore.status === 'loading') {
    await authStore.initializeFromCookie()
  }

  if (!authStore.isAuthenticated) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }

  return true
})

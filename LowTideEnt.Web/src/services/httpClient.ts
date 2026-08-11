import axios, { type InternalAxiosRequestConfig, type AxiosError } from 'axios'

// A dedicated axios instance so auth headers/interceptors don't leak into
// unrelated HTTP calls elsewhere in the app.
export const httpClient = axios.create({
  //baseURL: import.meta.env.VITE_API_BASE_URL,
  baseURL: 'http://localhost:8080/',
  timeout: 15000,
  withCredentials: true,
})

// axios's request config type doesn't have a `_retry` flag by default —
// extend it so we can track retries without using `any`.
interface RetryableRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean
}

type AccessTokenGetter = () => string | null
type UnauthorizedHandler = () => Promise<string>

// These are injected lazily (not imported directly) to avoid a circular
// dependency between httpClient -> auth store -> httpClient.
let getAccessToken: AccessTokenGetter = () => null
let onUnauthorized: UnauthorizedHandler = () => Promise.reject(new Error('Auth not configured'))

export function configureHttpClientAuth(options: {
  getAccessToken: AccessTokenGetter
  onUnauthorized: UnauthorizedHandler
}): void {
  getAccessToken = options.getAccessToken
  onUnauthorized = options.onUnauthorized
}

httpClient.interceptors.request.use((config) => {
  const token = getAccessToken()
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Queue concurrent requests that fail while a token refresh is already in
// flight, so we don't fire multiple parallel refresh calls.
interface PendingRequest {
  resolve: (token: string) => void
  reject: (error: unknown) => void
}

let isRefreshing = false
let pendingQueue: PendingRequest[] = []

function resolvePendingQueue(error: unknown, token: string | null = null) {
  pendingQueue.forEach(({ resolve, reject }) => {
    if (error) reject(error)
    else if (token) resolve(token)
  })
  pendingQueue = []
}

httpClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const { response, config } = error
    const retryableConfig = config as RetryableRequestConfig | undefined

    if (!response || response.status !== 401 || !retryableConfig || retryableConfig._retry) {
      return Promise.reject(error)
    }

    // Never attempt to refresh in response to the refresh call itself.
    if (retryableConfig.url?.includes('/auth/refresh')) {
      await onUnauthorized().catch(() => undefined)
      return Promise.reject(error)
    }

    if (isRefreshing) {
      return new Promise<string>((resolve, reject) => {
        pendingQueue.push({ resolve, reject })
      }).then((token) => {
        retryableConfig._retry = true
        retryableConfig.headers.Authorization = `Bearer ${token}`
        return httpClient(retryableConfig)
      })
    }

    retryableConfig._retry = true
    isRefreshing = true

    try {
      const newToken = await onUnauthorized()
      isRefreshing = false
      resolvePendingQueue(null, newToken)
      retryableConfig.headers.Authorization = `Bearer ${newToken}`
      return httpClient(retryableConfig)
    } catch (refreshError) {
      isRefreshing = false
      resolvePendingQueue(refreshError)
      return Promise.reject(refreshError)
    }
  },
)

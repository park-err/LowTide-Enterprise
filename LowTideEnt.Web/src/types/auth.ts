import type { PermissionSet } from './permissions'

export interface AuthUser {
  userName: string
  displayName: string
  avatarUrl: string
  roles: string[]
  permissions: PermissionSet[]
}

export interface CredentialPopupResponse {
  clientId: string
  /** JWT credential string */
  credential: string
  /** This field shows how the credential is selected */
  select_by:
    | 'auto'
    | 'user'
    | 'user_1tap'
    | 'user_2tap'
    | 'btn'
    | 'btn_confirm'
    | 'brn_add_session'
    | 'btn_confirm_add_session'
}

export type AuthStatus = 'idle' | 'loading' | 'auth_ready' | 'session_created' | 'error'

export interface PermissionSet {
  category: string
  permissions: Permission[]
}

interface Permission {
  name: string
  type: PermissionType[]
}

type PermissionType = 'View' | 'Create' | 'Edit'

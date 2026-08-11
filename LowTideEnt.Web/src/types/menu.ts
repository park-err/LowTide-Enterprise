export interface Menu {
  resourceMenu: MenuItem[]
}
export interface MenuItem {
  id: number
  title: string
  subMenu: MenuItem[]
}

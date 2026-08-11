export interface ResourceList {
  categoryId: number,
  categoryName: string,
  resources: Resource[] | null
  childList: ResourceList[] | null
}
interface Resource {
  id: number,
  title: string
}

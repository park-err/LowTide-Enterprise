import { httpClient } from '@/services/httpClient'
import type { ResourceList } from './types'

export const resourceService = {
  async fetchResourceList(categoryId: number): Promise<ResourceList> {
    console.log(categoryId)
    const { data } = await httpClient.get<ResourceList>(`/category/${categoryId}/resources/all`);
    return data;
  },
  async fetchResourceContent(categoryId: number, resourceId: number): Promise<string> {
    const { data } = await httpClient.get<string>(`/category/${categoryId}/resources/${resourceId}/content`)
    return data;
  }
}

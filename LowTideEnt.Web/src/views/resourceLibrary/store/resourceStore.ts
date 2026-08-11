import { defineStore } from 'pinia'
import { resourceService } from './resourceService'
import type { ResourceList } from './types'

interface ResourceState {
  resourceList: ResourceList | null
  resourceHtml: string | null
}

export const useResourceStore = defineStore('resource', {
  state: (): ResourceState => ({
    resourceList: null,
    resourceHtml: null
  }),
  getters: {

  },
  actions: {
    async fetchResources(categoryId: number): Promise<void> {
      const resourceList  = await resourceService.fetchResourceList(categoryId);
      this.resourceList = resourceList;
    },
    async fetchResourceContent(categoryId: number, resourceId: number): Promise<string> {
      const resourceHtml = await resourceService.fetchResourceContent(categoryId, resourceId);
      this.resourceHtml = resourceHtml
    }

  }
})

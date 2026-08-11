using LowTideEnt.Application.Services.Resource.Dto;

namespace LowTideEnt.Application.Interfaces
{
    public interface IResourceService
    {
        public Task<CategoryResponse?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken);
        public Task<CategoryResponse> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken);
        public Task UpdateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken);
        public Task RemoveCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
        public Task<ResourceResponse> GetResourceByIdAsync(int categoryId, int resourceId);
        public Task<string> GetResourceContentByIdAsync(int categoryId, int resourceId);
        public Task<ResourceListResponse> GetResourceListByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
        public Task<IEnumerable<ResourceResponse>> GetResourceByQueryAsync(int categoryId, ResourceQuery query, CancellationToken cancellationToken);
        public Task<ResourceResponse> AddResourceAsync(int categoryId, ResourceRequest request, CancellationToken cancellationToken);
        public Task UpdateResourceAsync(int categoryId, ResourceRequest request, CancellationToken cancellationToken);
        public Task RemoveResourceByIdAsync(int categoryId, int resourceId, CancellationToken cancellationToken);
    }
}

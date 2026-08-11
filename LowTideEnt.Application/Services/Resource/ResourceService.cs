using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Application.Services.Resource.Mapping;
using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Application.Services.Resource
{
    public class ResourceService : IResourceService
    {
        private ISessionContext sessionContext;
        private IResourceRepository resourceRepository;
        private ICategoryRepository categoryRepository;

        public ResourceService(ISessionContext sessionContext, IResourceRepository resourceRepository, ICategoryRepository categoryRepository)
        {
            this.sessionContext = sessionContext;
            this.resourceRepository = resourceRepository;
            this.categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetByCategoryIdAsync(id, cancellationToken);
            var topCategory = categories.Where(c => c.Id == id).FirstOrDefault();
            if (topCategory == null) throw new ExpectedEntityNotFoundException();
            var children = BuildCategoryHierarchy(topCategory.ToResponse(), categories);
            return topCategory.ToResponse(children);
        }
        public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllAsync(cancellationToken);
            IEnumerable<CategoryResponse> result = categories.Select(c => c.ToResponse()).Where(c => c.ParentId == null).ToList();
            foreach (var category in result)
            {
                category.ChildCategories = BuildCategoryHierarchy(category, categories);
            }
            return result;
        }
        public async Task<CategoryResponse> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.Name == null || request.Name == string.Empty) throw new InvalidRequestException();
            return (await categoryRepository.AddAsync(request.ToAddEntity(sessionContext.UserName), cancellationToken)).ToResponse();
        }
        public async Task UpdateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == 0 || request.Name == null || request.Name == string.Empty) throw new InvalidRequestException();
            await categoryRepository.UpdateAsync(request.ToUpdateEntity(sessionContext.UserName), cancellationToken);
        }
        public async Task RemoveCategoryByIdAsync(int categoryId, CancellationToken cancellationToken) =>
            await categoryRepository.RemoveCategoryByIdAsync(categoryId, sessionContext.UserName, cancellationToken);
        public async Task<ResourceResponse> GetResourceByIdAsync(int categoryId, int resourceId) =>
            (await resourceRepository.GetByIdAsync(categoryId, resourceId)).ToResponse();
        public async Task<string> GetResourceContentByIdAsync(int categoryId, int resourceId) =>
            (await resourceRepository.GetResourceContentByIdAsync(categoryId, resourceId)); 
        public async Task<ResourceListResponse> GetResourceListByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var resourceList = await resourceRepository.GetResourceListByCategoryIdsAsync(categoryId, cancellationToken);
            var resources = resourceList.Where(r => r.CategoryId == categoryId);
            return new ResourceListResponse
            {
                CategoryId = categoryId,
                CategoryName = resourceList.Where(c => c.CategoryId == categoryId).Select(c => c.CategoryName).FirstOrDefault() ?? string.Empty,
                ChildList = BuildResourceListHierarchy(categoryId, resourceList),
                Resources = resources.Count() > 0 ? resources.Select(r => r.ToResource()) : null
            };
        }
        public async Task<IEnumerable<ResourceResponse>> GetResourceByQueryAsync(int categoryId, ResourceQuery query, CancellationToken cancellationToken) =>
            (await resourceRepository.GetResourcesByQueryAsync(categoryId, query, cancellationToken))
            .Select(r => r.ToResponse()).ToList();
        public async Task<ResourceResponse> AddResourceAsync(int categoryId, ResourceRequest request, CancellationToken cancellationToken)
        {
            if (request.Title == null || request.Title == string.Empty) throw new InvalidRequestException();
            return (await resourceRepository.AddAsync(request.ToAddEntity(categoryId, sessionContext.UserName), cancellationToken)).ToResponse();
        }
        public async Task UpdateResourceAsync(int categoryId, ResourceRequest request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0 || request.Title == null || request.Title == string.Empty) throw new InvalidRequestException();
            await resourceRepository.UpdateAsync(request.ToUpdateEntity(categoryId, sessionContext.UserName), cancellationToken);
        }
            
        public async Task RemoveResourceByIdAsync(int categoryId, int resourceId, CancellationToken cancellationToken) =>
            await resourceRepository.RemoveResourceByIdAsync(categoryId, resourceId, sessionContext.UserName, cancellationToken);

        private IEnumerable<CategoryResponse>? BuildCategoryHierarchy(CategoryResponse result, IEnumerable<CategoryEntity> categories)
        {
            var children = categories.Select(c => c.ToResponse()).Where(c => c.ParentId == result.Id).ToList();

            foreach (var child in children)
            {
                child.ChildCategories = BuildCategoryHierarchy(child, categories);
            }
            return children.Count() == 0 ? null : children;
        }

        private IEnumerable<ResourceListResponse>? BuildResourceListHierarchy(int categoryId, IEnumerable<ResourceListModel> resourceList)
        {
            var children = resourceList.Where(r => r.ParentId == categoryId).Select(r => r.ToCategory()).Distinct().ToList();

            foreach (var child in children)
            {
                var resources = resourceList.Where(r => r.CategoryId == child.CategoryId);
                child.Resources = resources.Count() > 0 ? resources.Select(r => r.ToResource()).ToList() : null;
                child.ChildList = BuildResourceListHierarchy(child.CategoryId, resourceList);
            }
            return children.Count() == 0 ? null : children;
        }
    }
}

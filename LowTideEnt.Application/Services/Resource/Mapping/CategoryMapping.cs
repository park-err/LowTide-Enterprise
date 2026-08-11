using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Application.Services.Resource.Mapping
{
    public static class CategoryMapping
    {
        public static CategoryEntity ToAddEntity(this CategoryRequest request, string user) =>
            new CategoryEntity
            {
                Id = request.Id,
                StatusId = Status.Active,
                ParentId = request.ParentId,
                Name = request.Name,
                CreatedBy = user,
                CreatedDate = request.RequestDate,
                ModifiedBy = user,
                ModifiedDate = request.RequestDate
            };
        public static CategoryEntity ToUpdateEntity(this CategoryRequest request, string user) =>
            new CategoryEntity
            {
                Id = request.Id,
                StatusId = request.Status,
                ParentId = request.ParentId,
                Name = request.Name,
                ModifiedBy = user,
                ModifiedDate = request.RequestDate
            };
        public static CategoryResponse ToResponse(this CategoryEntity entity) =>
            new CategoryResponse(entity.Id, entity.Name, entity.ParentId);
        public static CategoryResponse ToResponse(this CategoryEntity entity, IEnumerable<CategoryResponse>? children) =>
            new CategoryResponse(entity.Id, entity.Name, entity.ParentId)
            {
                ChildCategories = children
            };
    }
}

using LowTideEnt.Application.Services.Resource.Dto;
using LowTideEnt.Domain.Entities.ResourceManager;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LowTideEnt.Application.Services.Resource.Mapping
{
    public static class ResourceMapping
    {
        public static ResourceResponse ToResponse(this ResourceModel model) =>
            new ResourceResponse
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Title = model.Title,
                Metadata = model.Metadata,
                ParentId = model.ParentId,
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = model.ModifiedDate,
            };
        public static ResourceResponse ToResponse(this ResourceEntity entity) =>
            new ResourceResponse
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Metadata = JsonSerializer.Deserialize<MetadataObject>(entity.Metadata) ?? new MetadataObject(),
                ModifiedBy = entity.ModifiedBy,
                ModifiedDate = entity.ModifiedDate,
            };
        public static ResourceEntity ToAddEntity(this ResourceRequest request, int categoryId, string user) =>
            new ResourceEntity(categoryId, request.Title, request.Content, request.Metadata)
            {
                StatusId = Status.Active,
                CreatedBy = user,
                CreatedDate = DateTime.Now,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now,
            };
        public static ResourceEntity ToUpdateEntity(this ResourceRequest request, int categoryId, string user) =>
            new ResourceEntity(categoryId, request.Title, request.Content, request.Metadata)
            {
                Id = request.Id,
                StatusId = Status.Active,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now,
            };
    }
}

using LowTideEnt.Application.Services.Resource.Dto;

namespace LowTideEnt.Application.Services.Resource.Mapping
{
    public static class ResourceListMapping
    {
        public static ResourceListResponse ToCategory(this ResourceListModel model) => 
            new ResourceListResponse { CategoryId = model.CategoryId, CategoryName = model.CategoryName };
        public static Dto.Resource ToResource(this ResourceListModel model) =>
            new Dto.Resource
            {
                Id = model.ResourceId ?? 0,
                Title = model.ResourceName ?? string.Empty
            };
        public static ResourceListResponse ToResponse(this ResourceListModel model, IEnumerable<Dto.Resource>? resources) =>
            new ResourceListResponse { CategoryId = model.CategoryId, CategoryName = model.CategoryName, Resources = resources };
    }
}

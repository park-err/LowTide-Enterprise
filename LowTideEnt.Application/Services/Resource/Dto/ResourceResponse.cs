using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Application.Services.Resource.Dto
{
    public class ResourceResponse : BaseResponse
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public required MetadataObject Metadata { get; set; }
    }
}

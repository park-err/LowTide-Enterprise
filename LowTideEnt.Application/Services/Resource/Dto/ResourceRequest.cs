using System.Text.RegularExpressions;
using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Application.Services.Resource.Dto
{
    public class ResourceRequest : BaseRequest
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required MetadataObject Metadata { get; set; }
    }
}

using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Domain.Models
{
    public class ResourceModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public required MetadataObject Metadata { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}

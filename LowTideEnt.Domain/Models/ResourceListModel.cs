namespace LowTideEnt.Domain.Models
{
    public class ResourceListModel
    {
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public required string CategoryName { get; set; }
        public int? ResourceId { get; set; }
        public string? ResourceName { get; set; }
    }
}

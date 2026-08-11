namespace LowTideEnt.Domain.Entities.ResourceManager
{
    [Table("Category", Schema = "ResourceManagement")]
    public class CategoryEntity : BaseEntity
    {
        public int? ParentId { get; set; }
        public required string Name { get; set; }
    }
}

namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("Role", Schema = "Admin")]
    public class RoleEntity : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}

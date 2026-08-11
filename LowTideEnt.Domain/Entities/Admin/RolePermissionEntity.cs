namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("RolePermission", Schema = "Admin")]
    public class RolePermissionEntity : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}

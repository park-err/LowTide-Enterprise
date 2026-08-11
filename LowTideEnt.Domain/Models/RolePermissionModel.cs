namespace LowTideEnt.Domain.Models
{
    public class RolePermissionModel
    {
        public required IEnumerable<RoleModel> Roles { get; set; }
        public IEnumerable<PermissionModel> Permissions { get; set; } = Enumerable.Empty<PermissionModel>();
    }

    public class RoleModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }

    public class PermissionModel
    {
        public required string Category { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
    }
}

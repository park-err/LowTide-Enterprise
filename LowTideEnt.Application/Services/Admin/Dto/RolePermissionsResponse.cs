namespace LowTideEnt.Application.Services.Admin.Dto
{
    public class RolePermissionsResponse
    {
        public string[] Roles { get; set; } = Array.Empty<string>();
        public IEnumerable<PermissionCategory> PermissionCategories = Enumerable.Empty<PermissionCategory>();
    }
    public class PermissionCategory
    {
        public string Category { get; set; } = string.Empty;
        public IEnumerable<Permission> Permissions { get; set; } = Enumerable.Empty<Permission>();
    }
    public class Permission
    {
        public string Name { get; set; } = string.Empty;
        public string[] Types { get; set; } = Array.Empty<string>();
    }
}

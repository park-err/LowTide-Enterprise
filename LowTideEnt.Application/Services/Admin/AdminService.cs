using LowTideEnt.Application.Authorization.Dto;
using LowTideEnt.Application.Services.Admin.Dto;
using LowTideEnt.Application.Services.Admin.Mapping;

namespace LowTideEnt.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        public IRoleRepository roleRepo;
        public IPermissionRepository permissionRepo;
        public ISessionContext sessionContext;
        public AdminService(ISessionContext sessionContext, IRoleRepository roleRepo, IPermissionRepository permissionRepo)
        {
            this.roleRepo = roleRepo;
            this.permissionRepo = permissionRepo;
            this.sessionContext = sessionContext;
        }
        public async Task<RolePermissionsResponse> GetRolePermissionsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var roles = await roleRepo.GetRolesByUserIdAsync(userId, cancellationToken);
            var roleIds = roles.Select(r => r.Id).ToArray();
            var permissions = await permissionRepo.GetPermissionsByRoleIdAsync(roleIds, cancellationToken) ?? Array.Empty<PermissionModel>();
            return new RolePermissionsResponse
            {
                Roles = roles.Select(r => r.Name).ToArray(),
                PermissionCategories = permissions.GroupBy(c => c.Category).Select(p => new PermissionCategory
                {
                    Category = p.Key,
                    Permissions = p.ToResponse()
                })
            };
        }
    }
}

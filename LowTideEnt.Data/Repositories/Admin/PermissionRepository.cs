using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public EnterpriseDbContext context;
        public PermissionRepository(EnterpriseDbContext context) => this.context = context;
        public async Task<IEnumerable<PermissionModel>?> GetPermissionsByRoleIdAsync(int[] roleIds, CancellationToken cancellationToken = default)
        {
            var schema = "Admin";
            var functionName = "GET_PERMISSIONS_BY_ROLE_IDS";
            var parameters = new Dictionary<string, object> {
                { "role_ids", roleIds }
            };
            return await context.ExecutePsqlFunctionAsync<IEnumerable<PermissionModel>>(functionName, parameters, cancellationToken, schema);
        }
    }
}

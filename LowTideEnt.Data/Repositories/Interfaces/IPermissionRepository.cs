using LowTideEnt.Domain.Entities.Admin;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<PermissionModel>?> GetPermissionsByRoleIdAsync(int[] roleIds, CancellationToken cancellationToken = default);
    }
}

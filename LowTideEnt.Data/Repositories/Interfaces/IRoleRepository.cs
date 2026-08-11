using LowTideEnt.Domain.Entities.Admin;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository : IEnterpriseRepository<RoleEntity>
    {
        Task<IEnumerable<RoleModel>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}

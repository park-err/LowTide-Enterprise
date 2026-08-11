using LowTideEnt.Domain.Entities.GlobalConfig;
using LowTideEnt.Domain.Queries;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IEnterpriseRepository<UserEntity>
    {
        Task<IEnumerable<UserEntity>> GetUsersByQueryAsync(UserQuery userQuery, CancellationToken cancellationToken = default);
        Task RemoveUserByIdAsync(int id, string user, CancellationToken cancellationToken = default);
    }
}

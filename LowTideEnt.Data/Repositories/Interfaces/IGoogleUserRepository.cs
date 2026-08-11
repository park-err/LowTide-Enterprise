using LowTideEnt.Domain.Entities.GlobalConfig;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IGoogleUserRepository : IEnterpriseRepository<UserEntity>
    {
        Task<UserEntity?> GetByGoogleIdAsync(GoogleAuthResponse googleUser, CancellationToken cancellationToken = default);
    }
}

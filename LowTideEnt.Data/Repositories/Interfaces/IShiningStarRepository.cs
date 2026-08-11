using LowTideEnt.Domain.Entities.Staff;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IShiningStarRepository : IEnterpriseRepository<ShiningStarEntity>
    {
        Task<ShiningStarEntity?> GetRecentShiningStarAsync(CancellationToken cancellationToken = default);
    }
}

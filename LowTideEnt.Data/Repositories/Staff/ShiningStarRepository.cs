using LowTideEnt.Domain.Entities.Staff;
using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories.Staff
{
    public class ShiningStarRepository : EnterpriseRepository<ShiningStarEntity>, IShiningStarRepository
    {
        public ShiningStarRepository(EnterpriseDbContext context) : base(context) { }
        public async Task<ShiningStarEntity?> GetRecentShiningStarAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
            .Where(c => c.StatusId == Domain.Status.Active)
            .OrderBy(c => c.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

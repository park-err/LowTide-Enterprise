using LowTideEnt.Domain.Entities.ResourceManager;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IEnterpriseRepository<CategoryEntity>
    {
        public Task<IEnumerable<CategoryEntity>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
        public Task RemoveCategoryByIdAsync(int categoryId, string user, CancellationToken cancellationToken = default);
    }
}

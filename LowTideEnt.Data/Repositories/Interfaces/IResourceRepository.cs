using LowTideEnt.Domain.Entities.ResourceManager;
using LowTideEnt.Domain.Queries;

namespace LowTideEnt.Infrastructure.Repositories.Interfaces
{
    public interface IResourceRepository : IEnterpriseRepository<ResourceEntity>
    {
        public new Task<ResourceEntity> GetByIdAsync(int resourceId);
        public Task<ResourceModel> GetByIdAsync(int categoryId, int resourceId);
        public Task<string> GetResourceContentByIdAsync(int categoryId, int resourceId, CancellationToken cancellationToken = default);
        public new Task<IEnumerable<ResourceModel>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<IEnumerable<ResourceListModel>> GetResourceListByCategoryIdsAsync(int categoryId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<ResourceModel>> GetResourcesByQueryAsync(int categoryId, ResourceQuery query, CancellationToken cancellationToken = default);
        public Task RemoveResourceByIdAsync(int categoryId, int resourceId, string user, CancellationToken cancellationToken = default);
    }
}

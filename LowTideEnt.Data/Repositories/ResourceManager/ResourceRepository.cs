using LowTideEnt.Domain.Entities.ResourceManager;
using LowTideEnt.Domain.Queries;
using LowTideEnt.Infrastructure.Data;


namespace LowTideEnt.Infrastructure.Repositories
{
    public class ResourceRepository : EnterpriseRepository<ResourceEntity>, IResourceRepository
    {
        public string Schema = "ResourceManagement";
        public ResourceRepository(EnterpriseDbContext context) : base(context) { }
        public async new Task<ResourceEntity> GetByIdAsync(int resourceId)
        {
            throw new NotImplementedException();
        }
        public async Task<ResourceModel> GetByIdAsync(int categoryId, int resourceId) 
        {
            var sql = "EXEC GET_RESOURCE_BY_ID @categoryId = {0}, @resourceId = {1}";
            var categoryIdParam = new SqlParameter("@categoryId", categoryId) { SqlDbType = SqlDbType.Int };
            var resourceIdParam = new SqlParameter("@resourceId", resourceId) { SqlDbType = SqlDbType.Int };
            ResourceModel resources = await _context.Set<ResourceModel>()
                .FromSqlRaw(sql, categoryIdParam, resourceIdParam)
                .FirstOrDefaultAsync() ?? throw new ExpectedEntityNotFoundException();
            return resources;
        }
        public async new Task<IEnumerable<ResourceModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ResourceListModel>> GetResourceListByCategoryIdsAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var functionName = "GET_RESOURCE_LIST_BY_CATEGORY_ID";
            var parameters = new Dictionary<string, object> {
                { "category_id", categoryId }
            };
            return await _context.ExecutePsqlFunctionAsync<IEnumerable<ResourceListModel>>(functionName, parameters, cancellationToken, Schema) ?? throw new ExpectedEntityNotFoundException();
        }
        public async Task<string> GetResourceContentByIdAsync(int categoryId, int resourceId, CancellationToken cancellationToken)
        {
            var content = await _context.Set<ResourceEntity>()
                .Where(r => r.CategoryId == categoryId && r.Id == resourceId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new ExpectedEntityNotFoundException();
            return content.HtmlContent;
        }
        public async Task<IEnumerable<ResourceModel>> GetResourcesByQueryAsync(int categoryId, ResourceQuery query, CancellationToken cancellationToken)
        {
            var sql = "EXEC GET_RESOURCES_BY_QUERY @category_id = {0}";
            var categoryIdParam = new SqlParameter("@categoryId", categoryId) { SqlDbType = SqlDbType.Int };
            IEnumerable<ResourceModel> resources = await _context.Set<ResourceModel>()
                .FromSqlRaw(sql, categoryIdParam)
                .ToListAsync(cancellationToken);
            return resources;
        }

        public async Task RemoveResourceByIdAsync(int categoryId, int resourceId, string user, CancellationToken cancellationToken)
        {
            var sql = "EXEC REMOVE_RESOURCE_BY_ID @category_id = {0}, @resource_id = {1}, @user = {2}";
            var categoryIdParam = new SqlParameter("categoryId", categoryId) { SqlDbType = SqlDbType.Int };
            var resourceIdParam = new SqlParameter("resourceId", resourceId) { SqlDbType = SqlDbType.Int };
            var userParam = new SqlParameter("user", user) { SqlDbType = SqlDbType.VarChar };

            await _context.Database.ExecuteSqlRawAsync(sql, categoryIdParam, resourceIdParam, userParam, cancellationToken);
        }
    }
}

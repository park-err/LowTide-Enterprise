using LowTideEnt.Domain.Entities.ResourceManager;
using LowTideEnt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LowTideEnt.Infrastructure.Repositories
{
    public class CategoryRepository : EnterpriseRepository<CategoryEntity>, ICategoryRepository
    {
        EnterpriseDbContext context;
        public CategoryRepository(EnterpriseDbContext context) : base(context) => this.context = context;

        public override async Task<CategoryEntity> GetByIdAsync(int categoryId, CancellationToken cancellationToken) => throw new NotImplementedException();
        public async Task<IEnumerable<CategoryEntity>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var functionName = "GET_CATEGORIES_BY_CATEGORY_ID";
            var parameters = new Dictionary<string, object> {
                { "category_id", categoryId }
            };
            return await context.ExecutePsqlFunctionAsync<IEnumerable<CategoryEntity>>(functionName, parameters, cancellationToken) ?? throw new ExpectedEntityNotFoundException();
        }

        public async Task RemoveCategoryByIdAsync(int categoryId, string user, CancellationToken cancellationToken)
        {
            var sql = "EXEC REMOVE_CATEGORY_BY_ID categoryId = {0}, user = {1}";
            var categoryIdParam = new SqlParameter("categoryId", categoryId) { SqlDbType = SqlDbType.Int };
            var userParam = new SqlParameter("user", user) { SqlDbType = SqlDbType.VarChar };

            await _context.Database.ExecuteSqlRawAsync(sql, categoryIdParam, userParam, cancellationToken);
        }
    }
}

using LowTideEnt.Infrastructure.Data;
using static LowTideEnt.Infrastructure.Middleware.ExceptionHandlerMiddleware;

namespace LowTideEnt.Infrastructure.Repositories
{
    public class EnterpriseRepository<T> : IEnterpriseRepository<T> where T : BaseEntity
    {
        protected readonly EnterpriseDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EnterpriseRepository(EnterpriseDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _dbSet.FindAsync(id) ?? throw new ExpectedEntityNotFoundException();

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) =>
            await _dbSet
            .Where(c => c.StatusId == Domain.Status.Active)
            .ToListAsync(cancellationToken);

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

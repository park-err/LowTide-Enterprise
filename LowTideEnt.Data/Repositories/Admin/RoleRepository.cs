using LowTideEnt.Domain.Entities.Admin;
using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories
{
    public class RoleRepository : EnterpriseRepository<RoleEntity>, IRoleRepository
    {
        public RoleRepository(EnterpriseDbContext context) : base(context) { }
        public async Task<IEnumerable<RoleModel>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var userToRoleTable = _context.Set<UserToRoleEntity>();
            var roles = await _dbSet
                .Join(userToRoleTable, r => r.Id, ur => ur.RoleId, (r, ur) => new { Role = r, UserToRole = ur })
                .Where(x => x.UserToRole.UserId == userId)
                .Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                })
                .ToListAsync(cancellationToken);
            return roles;
        }
    }
}

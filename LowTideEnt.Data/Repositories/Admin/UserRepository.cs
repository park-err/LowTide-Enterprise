using LowTideEnt.Domain.Entities.GlobalConfig;
using LowTideEnt.Domain.Queries;
using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories
{
    public class UserRepository : EnterpriseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(EnterpriseDbContext context) : base(context) { }
        public async Task<IEnumerable<UserEntity>> GetUsersByQueryAsync(UserQuery query, CancellationToken cancellationToken)
        {
            // TODO: Query details
            var sql = "EXEC GET_USERS_BY_QUERY";
            IEnumerable<UserEntity> users = await _context.Set<UserEntity>()
                .FromSqlRaw(sql)
                .ToListAsync(cancellationToken);
            return users;
        }
        public async Task RemoveUserByIdAsync(int id, string user, CancellationToken cancellationToken)
        {
            var sql = "EXEC REMOVE_USER_BY_ID @user_id = {0} @user = {1}";
            IEnumerable<UserEntity> users = await _context.Set<UserEntity>()
                .FromSqlRaw(sql)
                .ToListAsync(cancellationToken);
        }
    }
}

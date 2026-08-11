using LowTideEnt.Domain.Entities.GlobalConfig;
using LowTideEnt.Infrastructure.Data;
namespace LowTideEnt.Infrastructure.Repositories;

public class GoogleUserRepository : EnterpriseRepository<UserEntity>, IGoogleUserRepository
{
    public GoogleUserRepository(EnterpriseDbContext context) : base(context) { }

    public async Task<UserEntity?> GetByGoogleIdAsync(GoogleAuthResponse googleUser, CancellationToken cancellationToken)
    {
        var user = await _dbSet.Where(x => x.GoogleId == googleUser.GoogleId).FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            // if the user does not exist, find the user by email and update the GoogleId
            user = await UpdateUserByEmailAsync(googleUser, cancellationToken);
        }
        return user;
    }
    private async Task<UserEntity?> UpdateUserByEmailAsync(GoogleAuthResponse googleUser, CancellationToken cancellationToken)
    {
        var user = await _dbSet.Where(x => x.Email == googleUser.Email).FirstOrDefaultAsync(cancellationToken);
        if (user != null)
        {
            user.GoogleId = googleUser.GoogleId;
            user.DisplayName = googleUser.DisplayName;
            user.AvatarUrl = googleUser.AvatarUrl;
            user.ModifiedDate = DateTime.Now;
            _dbSet.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return user;
    }
}

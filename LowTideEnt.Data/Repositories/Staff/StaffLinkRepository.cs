using LowTideEnt.Domain.Entities.Staff;
using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories.Staff
{
    public class StaffLinkRepository : EnterpriseRepository<StaffLinkEntity>, IStaffLinkRepository
    {
        public StaffLinkRepository(EnterpriseDbContext context) : base(context) { }
    }
}

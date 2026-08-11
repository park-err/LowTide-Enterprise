using LowTideEnt.Domain.Entities.Staff;
using LowTideEnt.Infrastructure.Data;

namespace LowTideEnt.Infrastructure.Repositories.Staff
{
    public class AnnouncementRepository : EnterpriseRepository<AnnouncementEntity>, IAnnouncementRepository
    {
        public AnnouncementRepository(EnterpriseDbContext context) : base(context) { }
    }
}

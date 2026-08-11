namespace LowTideEnt.Domain.Queries
{
    public class UserQuery : BaseQuery
    {
        public int? RoleId { get; set; }
        public int? StatusId { get; set; }
        public string DisplayNameContains { get; set; } = string.Empty;
        public string EmailContains { get; set; } = string.Empty;
    }
}

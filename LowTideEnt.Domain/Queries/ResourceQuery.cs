namespace LowTideEnt.Domain.Queries
{
    public class ResourceQuery : BaseQuery
    {
        public int? ParentId { get; set; }
        public string ContentContains { get; set; } = string.Empty;
    }
}

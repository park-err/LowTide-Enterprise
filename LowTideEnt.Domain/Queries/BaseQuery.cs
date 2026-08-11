namespace LowTideEnt.Domain.Queries
{
    public class BaseQuery
    {
        public string CreatedByContains { get; set; } = string.Empty;
        public DateTime CreatedFromDate { get; set; } = DateTime.UnixEpoch;
        public DateTime CreatedToDate { get; set; } = DateTime.Now.AddDays(1);
        public string ModifiedByContains { get; set; } = string.Empty;
        public DateTime ModifiedFromDate { get; set; } = DateTime.UnixEpoch;
        public DateTime ModifiedToDate { get; set; } = DateTime.Now.AddDays(1);
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

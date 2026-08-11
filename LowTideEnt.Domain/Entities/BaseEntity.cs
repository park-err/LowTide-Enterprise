namespace LowTideEnt.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Status StatusId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set;} = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}

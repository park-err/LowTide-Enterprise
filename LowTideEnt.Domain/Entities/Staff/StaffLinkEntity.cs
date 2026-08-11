namespace LowTideEnt.Domain.Entities.Staff
{
    [Table("StaffLink", Schema = "Staff")]  
    public class StaffLinkEntity : BaseEntity
    {
        public required string Title { get; set; }
        public required string LinkUrl { get; set; }
    }
}

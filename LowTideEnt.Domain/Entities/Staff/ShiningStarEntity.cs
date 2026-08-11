namespace LowTideEnt.Domain.Entities.Staff
{
    [Table("ShiningStar", Schema = "Staff")]
    public class ShiningStarEntity : BaseEntity
    {
        public required string FullName { get; set; }
        public string? Quote { get; set; }
        public required string Value { get; set; }
    }
}

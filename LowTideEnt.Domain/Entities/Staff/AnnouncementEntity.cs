namespace LowTideEnt.Domain.Entities.Staff
{
    [Table("Announcement", Schema = "Staff")]
    public class AnnouncementEntity : BaseEntity
    {
        public required string Title { get; set; }
        public string? Body { get; set; }
        public string LinkUrl { get; set; } = "#";
    }
}

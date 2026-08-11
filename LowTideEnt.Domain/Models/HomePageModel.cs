namespace LowTideEnt.Domain.Models
{
    public class HomePageModel
    {
        public required ShiningStar ShiningStar { get; set; }
        public required IEnumerable<Announcement> Announcements { get; set; }
        public required IEnumerable<StaffLink> StaffLinks { get; set; }
    }

    public class Announcement
    {
        public required string Title { get; set; }
        public string? Body { get; set; }
        public string? LinkUrl { get; set; }
        public DateTime PostedDate { get; set; }
    }

    public class StaffLink
    {
        public required string Title { get; set; }
        public required string LinkUrl { get; set; }
    }

    public class ShiningStar
    {
        public required string FullName { get; set; }
        public string? Quote { get; set; }
        public required string Value { get; set; }
        public DateTime NominationDate { get; set; }
    }
}

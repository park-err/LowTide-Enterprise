using System.ComponentModel.Design;

namespace LowTideEnt.Application.Services.Home.Dto
{
    public class HomePageResponse
    {
        public HomePageResponse(ShiningStar shiningStar, IEnumerable<Announcement> announcements, IEnumerable<StaffLink> staffLinks)
        {
            ShiningStar = shiningStar;
            Announcements = announcements;
            StaffLinks = staffLinks;
        }
        public ShiningStar ShiningStar { get; set; }
        public IEnumerable<Announcement> Announcements { get; set; }
        public IEnumerable<StaffLink> StaffLinks { get; set; }
    }
}

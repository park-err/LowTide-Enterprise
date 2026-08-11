using LowTideEnt.Application.Services.Home.Dto;
using LowTideEnt.Application.Services.Home.Mapping;

namespace LowTideEnt.Application.Services.Home
{
    public class HomeService : IHomeService
    {
        IShiningStarRepository shiningStarRepository;
        IAnnouncementRepository announcementRepository;
        IStaffLinkRepository staffLinkRepository;
        IResourceService resourceService;
        public HomeService(IShiningStarRepository shiningStarRepository, IAnnouncementRepository announcementRepository,
            IStaffLinkRepository staffLinkRepository, IResourceService resourceService)
        {
            this.shiningStarRepository = shiningStarRepository;
            this.announcementRepository = announcementRepository;
            this.staffLinkRepository = staffLinkRepository;
            this.resourceService = resourceService;
        }
        public async Task<HomePageResponse> GetHomePageAsync(CancellationToken cancellationToken)
        {
            var shiningStar = (await shiningStarRepository.GetRecentShiningStarAsync(cancellationToken) ?? throw new ExpectedEntityNotFoundException()).ToResponse();
            var announcements = (await announcementRepository.GetAllAsync(cancellationToken)).Select(a => a.ToResponse());
            var staffLinks = (await staffLinkRepository.GetAllAsync(cancellationToken)).Select(a => a.ToResponse());

            return new HomePageResponse(shiningStar, announcements, staffLinks);
        }
        public async Task<MenuResponse> GetMenuAsync(CancellationToken cancellationToken)
        {
            var resourceResponse = await resourceService.GetCategoriesAsync(cancellationToken);
            MenuResponse menu = new MenuResponse();
            List<MenuItem> resourceSubMenu = new List<MenuItem>();
            foreach (var resourceItem in resourceResponse)
            {
                if (resourceItem.ChildCategories == null)
                {
                    continue;
                }
                var sub = resourceItem.ChildCategories.Select(c => c.CategoryToSubMenuItem());
                var item = resourceItem.CategoryToMenuItem(sub);
                resourceSubMenu.Add(item);
            }
            menu.ResourceMenu = resourceSubMenu.ToList();
            return menu;
        }
    }
}

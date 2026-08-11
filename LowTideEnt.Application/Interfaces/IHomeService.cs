using LowTideEnt.Application.Services.Home.Dto;

namespace LowTideEnt.Application.Interfaces
{
    public interface IHomeService
    {
        Task<HomePageResponse> GetHomePageAsync(CancellationToken cancellationToken);
        Task<MenuResponse> GetMenuAsync(CancellationToken cancellationToken);
    }
}

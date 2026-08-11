using LowTideEnt.Application.Interfaces;
using LowTideEnt.Application.Services.Home.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LowTideEnt.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController(IHomeService service) : ControllerBase
    {
        IHomeService service = service;
        [HttpGet]
        public async Task<ActionResult<HomePageResponse>> GetHomePage(CancellationToken cancellationToken)
        {
            var response = await service.GetHomePageAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("menu")]
        public async Task<ActionResult<MenuResponse>> GetMenu(CancellationToken cancellationToken)
        {
            var response = await service.GetMenuAsync(cancellationToken);
            return Ok(response);
        }
    }
}

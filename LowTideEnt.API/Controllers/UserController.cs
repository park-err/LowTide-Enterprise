using LowTideEnt.Application.Interfaces;
using LowTideEnt.Application.Services;
using LowTideEnt.Application.Services.User.Dto;
using LowTideEnt.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LowTideEnt.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService service;
        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await service.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByQuery([FromQuery] UserQuery query, CancellationToken cancellationToken)
        {
            var users = await service.GetUsersByQueryAsync(query, cancellationToken);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            var user = await service.AddUserAsync(request, cancellationToken);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            await service.UpdateUserAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("remove/{id}")]
        public async Task<IActionResult> RemoveUser(int id, CancellationToken cancellationToken)
        {
            await service.RemoveUserByIdAsync(id, cancellationToken);
            return Ok();
        }
    }
}


using LowTideEnt.Application.Authorization.Dto;
using LowTideEnt.Application.Interfaces;
using LowTideEnt.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LowTideEnt.Infrastructure.Middleware.ExceptionHandlerMiddleware;

namespace LowTideEnt.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService service) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<UserSessionResponse>> GetSession(CancellationToken cancellationToken)
        {
            var userDetails = await service.GetUserSessionAsync(cancellationToken);
            return Ok(userDetails);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Google([FromBody] AuthRequest request, CancellationToken cancellationToken)
        {
            var principal = await service.AuthenticateGoogleUserAsync(request.Credentials);
            if (principal == null)
            {
                throw new RequestedAccessException();
            }
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
            return Ok();
        }
    }
}

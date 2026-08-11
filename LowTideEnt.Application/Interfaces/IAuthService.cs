using LowTideEnt.Application.Authorization.Dto;
using System.Security.Claims;

namespace LowTideEnt.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal?> AuthenticateGoogleUserAsync(string idToken, CancellationToken cancellationToken = default);
        Task<UserSessionResponse> GetUserSessionAsync(CancellationToken cancellationToken = default);
    }
}

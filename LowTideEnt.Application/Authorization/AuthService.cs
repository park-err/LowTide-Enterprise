using LowTideEnt.Application.Authorization.Dto;
using LowTideEnt.Application.Authorization.Mapping;
using LowTideEnt.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using LowTideEnt.Domain.Entities.GlobalConfig;

namespace LowTideEnt.Application.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly IGoogleUserRepository repository;
        private readonly HttpClient http;
        private IHttpContextAccessor httpContext;
        private IUserService userService;
        private IAdminService adminService;
        private ISessionContext sessionContext;

        public AuthService(IGoogleUserRepository repository, HttpClient http, IHttpContextAccessor httpContext, 
            IUserService userService, IAdminService adminService, ISessionContext sessionContext)
        {
            this.repository = repository;
            this.http = http;
            this.httpContext = httpContext;
            this.userService = userService;
            this.adminService = adminService;
            this.sessionContext = sessionContext;
        }

        public async Task<ClaimsPrincipal?> AuthenticateGoogleUserAsync(string idToken, CancellationToken cancellationToken)
        {
            var googleUser = await GetGoogleUserAsync(idToken);
            var user = await repository.GetByGoogleIdAsync(googleUser, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return await GenerateClaimsPrincipal(user);
        }

        private async Task<GoogleAuthResponse> GetGoogleUserAsync(string idToken)
        {
            try
            {
                var response = await http.GetFromJsonAsync<GoogleTokenInfo>(
                    $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");

                if (response == null)
                    throw new AuthenticationException("Invalid Token");

                return response.ToResponse();
            }
            catch(HttpRequestException ex)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this application. Please contact the administator for more details.");
            }
        }

        private async Task<ClaimsPrincipal?> GenerateClaimsPrincipal(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // add user info to session
            httpContext.HttpContext?.Session.Set("UserId", Encoding.UTF8.GetBytes(user.Id.ToString()));
            httpContext.HttpContext?.Session.Set("UserName", Encoding.UTF8.GetBytes(user.UserName ?? "undefined"));

            return principal;
        }
        public async Task<UserSessionResponse> GetUserSessionAsync(CancellationToken cancellationToken)
        {
            var userId = sessionContext.UserId;
            var user = await userService.GetUserByIdAsync(userId);
            var rolesAndPermissions = await adminService.GetRolePermissionsByUserIdAsync(userId, cancellationToken);

            return new UserSessionResponse
            {
                UserName = user.UserName ?? "undefined",
                DisplayName = user.DisplayName ?? "undefined",
                AvatarUrl = user.AvatarUrl ?? "undefined",
                Roles = rolesAndPermissions.Roles,
                Permissions = rolesAndPermissions.PermissionCategories
            };
        }
    }
}

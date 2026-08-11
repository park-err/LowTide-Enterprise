using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LowTideEnt.Application.Services
{
    public class SessionContext : ISessionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public int UserId
        {
            get
            {
                var value = GetClaimValue(ClaimTypes.NameIdentifier) ?? GetClaimValue("UserId");
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException(
                        "UserId claim was not found on the current user. " +
                        "All API endpoints require authentication, so this indicates " +
                        "a misconfigured token or claims mapping rather than an anonymous request.");
                }
                return int.Parse(value);
            }
        }

        public string UserName
        {
            get
            {
                var value = GetClaimValue(ClaimTypes.Name) ?? GetClaimValue("UserName");

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException(
                        "UserName claim was not found on the current user. " +
                        "All API endpoints require authentication, so this indicates " +
                        "a misconfigured token or claims mapping rather than an anonymous request.");
                }

                return value;
            }
        }

        public string? Email =>
            GetClaimValue(ClaimTypes.Email) ?? GetClaimValue("UserEmail");

        public string? GetClaimValue(string claimType) =>
            User?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
}

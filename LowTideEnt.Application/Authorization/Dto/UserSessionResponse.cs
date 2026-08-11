using LowTideEnt.Application.Services.Admin.Dto;
namespace LowTideEnt.Application.Authorization.Dto
{
    public class UserSessionResponse
    {
        public required string UserName { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
        public IEnumerable<PermissionCategory> Permissions { get; set; }
    }
}

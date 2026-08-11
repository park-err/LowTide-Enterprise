namespace LowTideEnt.Application.Services.User.Dto
{
    public class UserResponse : BaseResponse
    {
        public Status StatusId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

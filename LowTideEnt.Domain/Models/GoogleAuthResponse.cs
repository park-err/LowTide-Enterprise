namespace LowTideEnt.Domain.Models
{
    public class GoogleAuthResponse
    {
        public GoogleAuthResponse(string googleId, string email, string? displayName, string? avatarUrl)
        {
            GoogleId = googleId;
            Email = email;
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
        }

        public string GoogleId { get; set; }
        public string Email { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
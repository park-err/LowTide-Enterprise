using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace LowTideEnt.Domain.Entities.GlobalConfig
{
    [Table("User", Schema = "GlobalConfig")]
    public class UserEntity : BaseEntity
    {
        [SetsRequiredMembers]
        public UserEntity()
        {
            GoogleId = string.Empty;
            Email = string.Empty;
            DisplayName = string.Empty;
            AvatarUrl = string.Empty;
        }
        [SetsRequiredMembers]
        public UserEntity(string googleId, string email, string avatarUrl = "")
        {
            GoogleId = googleId;
            Email = email;
            AvatarUrl = avatarUrl;
            GetUserName(email);
        }
        [SetsRequiredMembers]
        public UserEntity(Status status, string email, bool isAdmin, string user)
        {
            var date = DateTime.Now;
            StatusId = status;
            Email = email;
            IsAdmin = isAdmin;
            CreatedBy = user;
            CreatedDate = date;
            ModifiedBy = user;
            ModifiedDate = date;
        }
        public string? GoogleId { get; set; }
        public string? UserName { get; set; }
        public required string Email { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsAdmin { get; set; } = false;
        private void GetUserName(string? email)
        {
            if (email == null || email.Length == 0) { return; }
            UserName = email.Split("@")[0];
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("UserToRole", Schema = "Admin")]
    public class UserToRoleEntity
    {
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("PermissionName", Schema = "Admin")]
    public class PermissionNameEntity
    {
        [Key]
        public required int Id { get; set; }
        public required int PermissionCategoryId { get; set; }
        public required string Name { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

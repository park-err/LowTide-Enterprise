using System.ComponentModel.DataAnnotations;

namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("Permission", Schema = "Admin")]
    public class PermissionEntity
    {
        [Key]
        public int Id { get; set; }
        public Status StatusId { get; set; }
        public required int PermissionTypeId { get; set; }
        public required int PermissionNameId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LowTideEnt.Domain.Entities.Admin
{
    [Table("PermissionType", Schema = "Admin")]
    public class PermissionTypeEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}

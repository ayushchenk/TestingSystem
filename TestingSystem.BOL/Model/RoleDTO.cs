using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class RoleDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string RoleName { get; set; }
    }
}

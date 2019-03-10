using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Login { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(64)]
        public string LastName { get; set; }

        [StringLength(64)]
        public string Patronymic { get; set; }

        public int? GroupId { get; set; }

        public int? SpecializationId { get; set; }

        [Required]
        [StringLength(64)]
        public string GroupName { get; set; }

        [Required]
        [StringLength(128)]
        public string SpecializationName { get; set; }
    }
}

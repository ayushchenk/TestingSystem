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

        [StringLength(32)]
        public string GroupName { get; set; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        [StringLength(128)]
        public string EducationUnitName { get; set; }

        [Required]
        public int SpecializationId { set; get; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int EducationUnitId { set; get; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Login { get; set; }

        [Required]
        [MaxLength(40)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string Patronymic { get; set; }

        public int GroupId { get; set; }

        public int SpecializationId { get; set; }

        [MaxLength(10)]
        public string GroupName { get; set; }

        [MaxLength(40)]
        public string SpecializationName { get; set; }
    }
}

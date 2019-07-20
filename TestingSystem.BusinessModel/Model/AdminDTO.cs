using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BusinessModel.Model
{
    public class AdminDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        public bool IsGlobal { get; set; }

        public int? EducationUnitId { get; set; }

        [StringLength(128)]
        public string EducationUnitName { get; set; }
    }
}

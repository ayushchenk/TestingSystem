using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BusinessModel.Model
{
    public class SpecializationDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        public int? EducationUnitId { get; set; }
    }
}

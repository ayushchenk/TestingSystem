using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BusinessModel.Model
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        [DisplayName("Group")]
        public string GroupName { get; set; }

        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        [StringLength(128)]
        [DisplayName("Education unit")]
        public string EducationUnitName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SpecializationId { get; set; }

        public int EducationUnitId { get; set; }
    }
}

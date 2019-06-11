using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string GroupName { get; set; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        [StringLength(128)]
        public string EducationUnitName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SpecializationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int EducationUnitId { get; set; }
    }
}

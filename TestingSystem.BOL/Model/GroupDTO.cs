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

        [Required]
        public int SpecializationId { get; set; }

        [Required]
        public int EducationUnitId { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BusinessModel.Model
{
    public class EducationUnitDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [DisplayName("Education unit")]
        public string EducationUnitName { get; set; }
    }
}

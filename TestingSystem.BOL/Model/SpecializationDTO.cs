using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class SpecializationDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string SpecializationName { get; set; }
    }
}

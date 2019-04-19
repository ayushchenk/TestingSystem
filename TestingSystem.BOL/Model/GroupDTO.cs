using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string GroupName { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        public string SpecializationName { set; get; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string QuestionString { get; set; }

        [StringLength(128)]
        [DisplayName("Test")]
        public string TestName { get; set; }

        [StringLength(256)]
        public string ImagePath { get; set; }

        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        public int? QuestionImageId { get; set; }

        public int SpecializationId { get; set; }

        public int EducationUnitId { get; set; }

        public int SubjectId { get; set; }
    }
}

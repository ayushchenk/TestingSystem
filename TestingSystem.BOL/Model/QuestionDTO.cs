using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionString { get; set; }

        [StringLength(128)]
        public string TestName { get; set; }

        [StringLength(256)]
        public string ImagePath { get; set; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        [StringLength(32)]
        public string SubjectName { get; set; }

        public int? QuestionImageId { get; set; }

        public int SpecializationId { get; set; }

        public int SubjectId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionAnswerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string AnswerString { get; set; }

        public bool IsCorrect { get; set; }

        [StringLength(256)]
        public string QuestionString { get; set; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        public int QuestionId { get; set; }
    }
}

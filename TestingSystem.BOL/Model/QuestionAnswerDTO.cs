using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionAnswerDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string AnswerString { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public int TestId { set; get; }

        [Required]
        [MaxLength(500)]
        public string QuestionString { get; set; }

        [Required]
        [MaxLength(20)]
        public string TestName { get; set; }
    }
}

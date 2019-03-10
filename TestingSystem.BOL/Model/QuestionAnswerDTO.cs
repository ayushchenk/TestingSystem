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

        public int QuestionId { get; set; }

        public int TestId { set; get; }

        [Required]
        [StringLength(256)]
        public string QuestionString { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionAnswerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string AnswerString { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public PickedCheckbox PickedCheckbox { set; get; }
    }

    public struct PickedCheckbox
    {
        public int AnswerId { set; get; }
        public bool Checked { set; get; }
    }
}

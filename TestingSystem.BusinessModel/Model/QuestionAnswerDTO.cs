using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BusinessModel.Model
{
    public class QuestionAnswerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
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

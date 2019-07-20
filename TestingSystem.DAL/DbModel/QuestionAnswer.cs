namespace TestingSystem.DAL.DbModel
{
    using System.ComponentModel.DataAnnotations;

    public partial class QuestionAnswer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string AnswerString { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}

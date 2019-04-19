namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.question_answers")]
    public partial class QuestionAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("answer_string")]
        public string AnswerString { get; set; }

        [Required]
        [Column("is_correct")]
        public bool IsCorrect { get; set; }

        [Required]
        [Column("question_id")]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}

namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuestionAnswer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string AnswerString { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}

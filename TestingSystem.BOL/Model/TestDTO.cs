using System;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class TestDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }

        public bool IsOpen { get; set; }

        public int QuestionCount { set; get; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        public int SpecializationId { get; set; }
    }
}

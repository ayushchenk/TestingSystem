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

        [Range(1, 100, ErrorMessage = "Please enter number between 1 and 100")]
        public int QuestionCount { set; get; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        [StringLength(32)]
        public string SubjectName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SpecializationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SubjectId { get; set; }
    }
}

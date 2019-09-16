using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class TestDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [DisplayName("Test")]
        public string TestName { get; set; }

        [Range(1, 50, ErrorMessage = "Please enter number between 1 and 50")]
        public int EasyCount { get; set; }

        [Range(1, 50, ErrorMessage = "Please enter number between 1 and 50")]
        public int MediumCount { get; set; }

        [Range(1, 50, ErrorMessage = "Please enter number between 1 and 50")]
        public int HardCount { get; set; }

        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        public int TeacherId { set; get; }

        public int SubjectId { get; set; }

        public bool IsDeleted { get; set; }

        public int SpecializationId { get; set; }

        public int EducationUnitId { get; set; }
    }
}

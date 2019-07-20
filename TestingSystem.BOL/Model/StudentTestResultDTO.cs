using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class StudentTestResultDTO
    {
        public int Id { get; set; }

        public int Result { get; set; }

        public int QuestionCount { get; set; }

        [StringLength(32)]
        [DisplayName("Group")]
        public string GroupName { get; set; }

        [StringLength(128)]
        [DisplayName("Test")]
        public string TestName { get; set; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int GroupInTestId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int GroupId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int TestId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SubjectId { set; get; }
    }
}

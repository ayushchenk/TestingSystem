using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class TeachersInGroupDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int TeacherInSubjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int GroupId { get; set; }

        [StringLength(32)]
        [DisplayName("Group")]
        public string GroupName { get; set; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        [StringLength(128)]
        [DisplayName("Education unit")]
        public string EducationUnitName { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(64)]
        public string LastName { get; set; }

        public int SubjectId { set; get; }

        public int TeacherId { set; get; }

        public int SpecializationId { get; set; }

        public int TeacherSpecialization { get; set; }

        public int EducationUnitId { get; set; }
    }
}

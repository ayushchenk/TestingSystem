using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.BOL.Model
{
    public class TeacherDTO
    {
        public int Id { set; get; }

        [Required]
        [StringLength(64)]
        public string Login { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        [StringLength(64)]
        [DisplayName("Specialization")]
        public string SpecializationName { get; set; }

        [StringLength(128)]
        [DisplayName("Education unit")]
        public string EducationUnitName { get; set; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        public int SpecializationId { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SubjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int EducationUnitId { set; get; }

        public string FullName
        {
            get { return $"{LastName} {FirstName} - {SubjectName}"; } 
        }
    }
}

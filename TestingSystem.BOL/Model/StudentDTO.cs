using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class StudentDTO
    {
        public int Id { get; set; }

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
        public string GroupName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int GroupId { get; set; }

        public int SpecializationId { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int EducationUnitId { get; set; }
    }
}

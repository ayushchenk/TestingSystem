using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string SubjectName { get; set; }

        [StringLength(64)]
        public string SpecializationName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SpecializationId { get; set; }
    }
}

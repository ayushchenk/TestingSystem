using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class EducationUnitDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string EducationUnitName { get; set; }
    }
}

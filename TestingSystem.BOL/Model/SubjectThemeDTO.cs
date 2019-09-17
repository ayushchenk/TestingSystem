using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class SubjectThemeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string ThemeName { get; set; }

        [StringLength(64)]
        public string SubjectName { get; set; }

        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public int Questions { set; get; }
    }
}

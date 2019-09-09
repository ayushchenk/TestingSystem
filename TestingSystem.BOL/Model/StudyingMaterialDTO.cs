using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class StudyingMaterialDTO
    {
        public int Id { set; get; }

        [Required]
        [StringLength(64)]
        [DisplayName("Material name")]
        public string StudyingMaterialName { set; get; }

        [Required]
        [StringLength(384)]
        public string Link { set; get; }

        [StringLength(32)]
        [DisplayName("Subject")]
        public string SubjectName { get; set; }

        public int TeacherId { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int SubjectId { set; get; }

        public int SpecializationId { set; get; }
    }
}

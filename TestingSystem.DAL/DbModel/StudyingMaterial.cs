using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.DAL.DbModel
{
    public partial class StudyingMaterial
    {
        public int Id { set; get; }

        [Required]
        [StringLength(64)]
        public string StudyingMaterialName { set; get; }

        [Required]
        [StringLength(384)]
        public string Link { set; get; }

        public int TeacherId { set; get; }

        public virtual Teacher Teacher { set; get; }
    }
}

namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudyingMaterial
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string StudyingMaterialName { get; set; }

        [Required]
        [StringLength(384)]
        public string Link { get; set; }

        public int TeacherId { get; set; }

        public int SubjectId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual Subject Subject { get; set; }
    }
}

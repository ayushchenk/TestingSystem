namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubjectTheme
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubjectTheme()
        {
            Questions = new HashSet<Question>();
            ThemesInTests = new HashSet<ThemesInTest>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string ThemeName { get; set; }

        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemesInTest> ThemesInTests { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}

namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            GroupsInTests = new HashSet<GroupsInTest>();
            ThemesInTests = new HashSet<ThemesInTest>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }

        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public bool IsDeleted { get; set; }

        public int EasyCount { get; set; }

        public int MediumCount { get; set; }

        public int HardCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupsInTest> GroupsInTests { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual Teacher Teacher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemesInTest> ThemesInTests { get; set; }
    }
}

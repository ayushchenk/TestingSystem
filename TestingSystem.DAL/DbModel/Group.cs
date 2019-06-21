namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            GroupsInTests = new HashSet<GroupsInTest>();
            Students = new HashSet<Student>();
            TeachersInGroups = new HashSet<TeachersInGroup>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string GroupName { get; set; }

        public int SpecializationId { get; set; }

        public int EducationUnitId { get; set; }

        public virtual EducationUnit EducationUnit { get; set; }

        public virtual Specialization Specialization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupsInTest> GroupsInTests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeachersInGroup> TeachersInGroups { get; set; }
    }
}

namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Admin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Login { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        public bool IsGlobal { get; set; }

        public int? EducationUnitId { get; set; }

        public virtual EducationUnit EducationUnit { get; set; }
    }
}

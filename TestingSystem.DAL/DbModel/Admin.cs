namespace TestingSystem.DAL.DbModel
{
    using System.ComponentModel.DataAnnotations;

    public partial class Admin
    {
        public int Id { get; set; }

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

namespace TestingSystem.DAL.DbModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionString { get; set; }

        public int? QuestionImageId { get; set; }

        public int SubjectId { get; set; }

        public int EducationUnitId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual QuestionImage QuestionImage { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual EducationUnit EducationUnit { get; set; }
    }
}

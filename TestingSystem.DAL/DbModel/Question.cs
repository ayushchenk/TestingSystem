namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(2048)]
        public string QuestionString { get; set; }

        public int? QuestionImageId { get; set; }

        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public int Difficulty { get; set; }

        public int ThemeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual QuestionImage QuestionImage { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual SubjectTheme SubjectTheme { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}

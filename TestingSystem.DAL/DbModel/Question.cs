namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.questions")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("question_string")]
        public string QuestionString { get; set; }

        [Column("image_id")]
        public int? QuestionImageId { get; set; }

        [Required]
        [Column("specialization_id")]
        public int SpecializationId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual QuestionImage QuestionImage { get; set; }

        public virtual Specialization Specialization { get; set; }
    }
}

namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.groups_in_tests")]
    public partial class GroupsInTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("length")]
        public int Length { get; set; }

        [Required]
        [Column("group_id")]
        public int GroupId { get; set; }

        [Required]
        [Column("test_id")]
        public int TestId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Test Test { get; set; }
    }
}

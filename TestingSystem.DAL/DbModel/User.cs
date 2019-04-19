namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.users")]
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("login")]
        public string Login { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(15)]
        [Column("firstname")]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [Column("lastname")]
        public string LastName { get; set; }

        [MaxLength(20)]
        [Column("patronymic")]
        public string Patronymic { get; set; }

        [Required]
        [Column("group_id")]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}

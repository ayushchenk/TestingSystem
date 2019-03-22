namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroupsInTest
    {
        public int Id { get; set; }

        public TimeSpan StartTime { get; set; }

        public int GroupId { get; set; }

        public int TestId { get; set; }

        public int Length { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        public virtual Group Group { get; set; }

        public virtual Test Test { get; set; }
    }
}

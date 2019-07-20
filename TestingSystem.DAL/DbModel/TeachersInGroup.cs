namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeachersInGroup
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}

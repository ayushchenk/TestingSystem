namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentTestResult
    {
        public int Id { get; set; }

        public int Result { get; set; }

        public int GroupInTestId { get; set; }

        public int StudentId { get; set; }

        public virtual GroupsInTest GroupsInTest { get; set; }

        public virtual Student Student { get; set; }
    }
}

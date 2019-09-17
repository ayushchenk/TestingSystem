namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ThemesInTest
    {
        public int Id { get; set; }

        public int ThemeId { get; set; }

        public int TestId { get; set; }

        public virtual SubjectTheme SubjectTheme { get; set; }

        public virtual Test Test { get; set; }
    }
}

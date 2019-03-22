namespace TestingSystem.DAL.DbModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestingSystemContext : DbContext
    {
        public TestingSystemContext()
            : base("name=TestingSystemContext")
        {
            if (!Database.Exists())
                Database.SetInitializer(new DbInitialize.DbInitialize());
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupsInTest> GroupsInTests { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<QuestionImage> QuestionImages { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupsInTests)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionImage>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.QuestionImage)
                .HasForeignKey(e => e.ImageId);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionAnswers)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Tests)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.GroupsInTests)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);
        }
    }
}

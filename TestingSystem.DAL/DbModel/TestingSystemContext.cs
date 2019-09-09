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
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<EducationUnit> EducationUnits { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupsInTest> GroupsInTests { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<QuestionImage> QuestionImages { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentTestResult> StudentTestResults { get; set; }
        public virtual DbSet<StudyingMaterial> StudyingMaterials { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeachersInGroup> TeachersInGroups { get; set; }
        public virtual DbSet<TeachersInSubject> TeachersInSubjects { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EducationUnit>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.EducationUnit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EducationUnit>()
                .HasMany(e => e.Teachers)
                .WithRequired(e => e.EducationUnit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupsInTests)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupsInTest>()
                .HasMany(e => e.StudentTestResults)
                .WithRequired(e => e.GroupsInTest)
                .HasForeignKey(e => e.GroupInTestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Subjects)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Teachers)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.StudyingMaterials)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.TeachersInSubjects)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Tests)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.QuestionImages)
                .WithRequired(e => e.Teacher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Teacher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.StudyingMaterials)
                .WithRequired(e => e.Teacher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.TeachersInSubjects)
                .WithRequired(e => e.Teacher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.Tests)
                .WithRequired(e => e.Teacher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TeachersInSubject>()
                .HasMany(e => e.TeachersInGroups)
                .WithRequired(e => e.TeachersInSubject)
                .HasForeignKey(e => e.TeacherInSubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.GroupsInTests)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);
        }
    }
}

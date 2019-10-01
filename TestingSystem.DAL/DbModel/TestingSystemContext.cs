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
        public virtual DbSet<SubjectTheme> SubjectThemes { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeachersInGroup> TeachersInGroups { get; set; }
        public virtual DbSet<TeachersInSubject> TeachersInSubjects { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<ThemesInTest> ThemesInTests { get; set; }

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
                .HasMany(e => e.Students)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupsInTest>()
                .HasMany(e => e.StudentTestResults)
                .WithRequired(e => e.GroupsInTest)
                .HasForeignKey(e => e.GroupInTestId);

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
                .HasMany(e => e.SubjectThemes)
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

            modelBuilder.Entity<SubjectTheme>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.SubjectTheme)
                .HasForeignKey(e => e.ThemeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubjectTheme>()
                .HasMany(e => e.ThemesInTests)
                .WithRequired(e => e.SubjectTheme)
                .HasForeignKey(e => e.ThemeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TeachersInSubject>()
                .HasMany(e => e.TeachersInGroups)
                .WithRequired(e => e.TeachersInSubject)
                .HasForeignKey(e => e.TeacherInSubjectId);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.ThemesInTests)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);
        }
    }
}

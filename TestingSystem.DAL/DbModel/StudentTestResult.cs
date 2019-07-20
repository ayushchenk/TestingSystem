namespace TestingSystem.DAL.DbModel
{
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

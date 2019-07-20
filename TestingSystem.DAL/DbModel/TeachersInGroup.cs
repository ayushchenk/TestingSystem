namespace TestingSystem.DAL.DbModel
{
    public partial class TeachersInGroup
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}

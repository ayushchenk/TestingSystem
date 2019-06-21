using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class TeacherRepository : GenericRepository.Common.GenericRepository<Teacher>
    {
        public TeacherRepository(DbContext context) : base(context)
        {
        }
    }
}

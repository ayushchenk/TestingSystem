using GenericRepository.Common;
using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class StudentRespository : GenericRepository<Student>
    {
        public StudentRespository(DbContext context) : base(context)
        {
        }
    }
}

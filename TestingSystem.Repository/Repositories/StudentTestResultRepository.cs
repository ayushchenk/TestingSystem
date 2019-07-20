using GenericRepository.Common;
using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class StudentTestResultRepository : GenericRepository<StudentTestResult>
    {
        public StudentTestResultRepository(DbContext context) : base(context)
        {
        }
    }
}

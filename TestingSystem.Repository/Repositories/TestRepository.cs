using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class TestRepository : GenericRepository.Common.GenericRepository<Test>
    {
        public TestRepository(DbContext context) : base(context)
        {
        }
    }
}

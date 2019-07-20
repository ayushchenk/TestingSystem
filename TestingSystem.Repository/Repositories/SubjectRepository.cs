using GenericRepository.Common;
using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository(DbContext context) : base(context)
        {
        }
    }
}

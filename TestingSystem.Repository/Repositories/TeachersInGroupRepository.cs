using GenericRepository.Common;
using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class TeachersInGroupRepository : GenericRepository<TeachersInGroup>
    {
        public TeachersInGroupRepository(DbContext context) : base(context)
        {
        }
    }
}

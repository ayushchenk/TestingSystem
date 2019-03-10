using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class GroupRepository : GenericRepository.Common.GenericRepository<Group>
    {
        public GroupRepository(DbContext context) : base(context)
        {
        }
    }
}

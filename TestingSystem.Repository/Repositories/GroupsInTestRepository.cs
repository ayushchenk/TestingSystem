using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class GroupsInTestRepository : GenericRepository.Common.GenericRepository<GroupsInTest>
    {
        public GroupsInTestRepository(DbContext context) : base(context)
        {
        }
    }
}

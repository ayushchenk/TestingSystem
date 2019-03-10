using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class UserRepository : GenericRepository.Common.GenericRepository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}

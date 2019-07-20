using GenericRepository.Common;
using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class AdminRepository : GenericRepository<Admin>
    {
        public AdminRepository(DbContext context) : base(context)
        {
        }
    }
}

using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class RoleRepository : GenericRepository.Common.GenericRepository<Role>
    {
        public RoleRepository(System.Data.Entity.DbContext context) : base(context)
        {
        }
    }
}

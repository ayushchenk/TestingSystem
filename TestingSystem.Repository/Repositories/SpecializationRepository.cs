using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class SpecializationRepository : GenericRepository.Common.GenericRepository<Specialization>
    {
        public SpecializationRepository(DbContext context) : base(context)
        {
        }
    }
}

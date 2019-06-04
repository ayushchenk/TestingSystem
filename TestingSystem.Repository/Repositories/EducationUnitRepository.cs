using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class EducationUnitRepository : GenericRepository.Common.GenericRepository<EducationUnit>
    {
        public EducationUnitRepository(DbContext context) : base(context)
        {
        }
    }
}

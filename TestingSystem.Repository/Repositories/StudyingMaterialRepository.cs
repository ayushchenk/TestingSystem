using System.Data.Entity;
using TestingSystem.DAL.DbModel;
using GenericRepository.Common;

namespace TestingSystem.Repository.Repositories
{
    public class StudyingMaterialRepository : GenericRepository<StudyingMaterial>
    {
        public StudyingMaterialRepository(DbContext context) : base(context)
        {
        }
    }
}

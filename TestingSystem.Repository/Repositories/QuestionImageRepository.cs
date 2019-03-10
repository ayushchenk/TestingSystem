using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class QuestionImageRepository : GenericRepository.Common.GenericRepository<QuestionImage>
    {
        public QuestionImageRepository(DbContext context) : base(context)
        {
        }
    }
}

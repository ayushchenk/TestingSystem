using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class QuestionRepository : GenericRepository.Common.GenericRepository<Question>
    {
        public QuestionRepository(DbContext context) : base(context)
        {
        }
    }
}

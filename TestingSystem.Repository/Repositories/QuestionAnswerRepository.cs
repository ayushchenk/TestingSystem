using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class QuestionAnswerRepository : GenericRepository.Common.GenericRepository<QuestionAnswer>
    {
        public QuestionAnswerRepository(DbContext context) : base(context)
        {
        }
    }
}

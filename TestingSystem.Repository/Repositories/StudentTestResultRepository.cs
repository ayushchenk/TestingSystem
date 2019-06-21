using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.DAL.DbModel;
using System.Data.Entity;

namespace TestingSystem.Repository.Repositories
{
    public class StudentTestResultRepository : GenericRepository<StudentTestResult>
    {
        public StudentTestResultRepository(DbContext context) : base(context)
        {
        }
    }
}

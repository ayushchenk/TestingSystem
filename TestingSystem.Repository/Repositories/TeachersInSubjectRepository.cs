using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.Repository.Repositories
{
    public class TeachersInSubjectRepository : GenericRepository<TeachersInSubject>
    {
        public TeachersInSubjectRepository(DbContext context) : base(context)
        {
        }
    }
}

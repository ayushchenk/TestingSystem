using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.DAL.DbInitialize
{
    public class DbInitialize: DropCreateDatabaseIfModelChanges<TestingSystemContext>
    {
        protected override void Seed(TestingSystemContext context)
        {
            base.Seed(context);
        }
    }
}

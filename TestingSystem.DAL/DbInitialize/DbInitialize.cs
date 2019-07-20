using System.Data.Entity;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.DAL.DbInitialize
{
    public class DbInitialize : DropCreateDatabaseIfModelChanges<TestingSystemContext>
    {
        protected override void Seed(TestingSystemContext context)
        {
            base.Seed(context);
        }
    }
}

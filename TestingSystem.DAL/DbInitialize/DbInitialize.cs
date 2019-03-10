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
            if(!context.Roles.Any( role => role.RoleName == "Admin"))
            {
                context.Roles.Add(new DbModel.Role { RoleName = "Admin"});
                context.SaveChanges();
            }
            base.Seed(context);
        }
    }
}

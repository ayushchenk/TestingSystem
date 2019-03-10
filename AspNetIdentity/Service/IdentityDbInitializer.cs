using AspNetIdentity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetIdentity.Service
{
    public class IdentityDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var role = context.Roles.Add(new AppRole() { Name = "Admin" });

            base.Seed(context);
        }
    }
}

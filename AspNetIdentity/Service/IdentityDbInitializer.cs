using AspNetIdentity.Models;
using System.Data.Entity;

namespace AspNetIdentity.Service
{
    public class IdentityDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //var role = context.Roles.Add(new AppRole() { Name = "Admin" });

            base.Seed(context);
        }
    }
}

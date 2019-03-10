using AspNetIdentity.Service;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public ApplicationDbContext(string name = "AspNetIdentity") : base(name) 
        {
            System.Data.Entity.Database.SetInitializer(new IdentityDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

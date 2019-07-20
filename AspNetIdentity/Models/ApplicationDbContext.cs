using AspNetIdentity.Service;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace AspNetIdentity.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public ApplicationDbContext()
            : base("AspNetIdentity") { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //static ApplicationDbContext()
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        //}

    }

    //internal class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    //{
    //    public Configuration()
    //    {
    //        AutomaticMigrationsEnabled = true;
    //        SetSqlGenerator("Devart.Data.PostgreSql", new Devart.Data.PostgreSql.Entity.Migrations.PgSqlEntityMigrationSqlGenerator());
    //    }
    //}
}

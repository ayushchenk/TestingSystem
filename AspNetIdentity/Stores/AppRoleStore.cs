using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace AspNetIdentity.Stores
{
    public class AppRoleStore : RoleStore<AppRole, int, AppUserRole>
    {
        public AppRoleStore(DbContext context) : base(context) { }

        public override Task UpdateAsync(AppRole role)
        {
            return Task.Run(() =>
            {
                var set = Context.Set<AppRole>();
                set.AddOrUpdate(role);
                Context.SaveChanges();
            });
        }
    }
}

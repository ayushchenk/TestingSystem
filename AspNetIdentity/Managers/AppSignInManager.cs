using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin;

namespace AspNetIdentity.Managers
{
    public class AppSignInManager : SignInManager<AppUser, int>
    {
        public AppSignInManager(UserManager<AppUser, int> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager) { }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
}

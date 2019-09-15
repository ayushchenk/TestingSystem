using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Global Admin, Education Unit Admin")]
    public class AdminContentController : Controller
    {
        private AdminDTO admin;
        private IEntityService<AdminDTO> adminService;

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        public AdminContentController(IEntityService<AdminDTO> adminService)
        {
            this.adminService = adminService;
        }

        public ActionResult Index()
        {
            if (this.Admin.IsGlobal)
                return RedirectToAction("Index", "Role");
            else
                return RedirectToAction("Index", "Group");
        }

        public new async Task<ActionResult> Profile()
        {
            if (this.Admin == null)
                return RedirectToAction("Index");
            var appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (appUser == null)
                return RedirectToAction("Index");
            var model = new EditAdminViewModel
            {
                Admin = this.Admin,
                IsTwoFactorEnabled = appUser.TwoFactorEnabled
            };
            return View(model);
        }

        public async Task<ActionResult> Edit()
        {
            if (this.Admin == null)
                return RedirectToAction("Index");
            var appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (appUser == null)
                return RedirectToAction("Index");
            var model = new EditAdminViewModel
            {
                Admin = this.Admin,
                IsTwoFactorEnabled = appUser.TwoFactorEnabled
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
                if (appUser != null)
                {
                    appUser.Email = model.Admin.Email;
                    appUser.UserName = model.Admin.Email;
                    appUser.TwoFactorEnabled = model.IsTwoFactorEnabled;
                    await UserManager.UpdateAsync(appUser);
                    await adminService.AddOrUpdateAsync(model.Admin);
                }
                return RedirectToAction("Profile");
            }
            return View(model);
        }
    }
}
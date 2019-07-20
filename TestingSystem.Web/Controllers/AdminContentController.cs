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

        public new ActionResult Profile()
        {
            if (this.Admin == null)
                return RedirectToAction("Index");
            return View(this.Admin);
        }

        public ActionResult Edit()
        {
            if (this.Admin == null)
                return RedirectToAction("Index");
            return View(this.Admin);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AdminDTO model)
        {
            if (ModelState.IsValid && this.Admin != null)
            {
                AdminDTO oldUser = this.Admin;
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Email;
                        await UserManager.UpdateAsync(appUser);
                        await adminService.AddOrUpdateAsync(model);
                        return RedirectToAction("Profile");
                    }
                }
            }
            return View(model);
        }
    }
}
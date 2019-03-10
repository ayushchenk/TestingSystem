using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await RoleManager.FindByIdAsync(id) ?? new AppRole();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AppRole role)
        {
            if (ModelState.IsValid)
            {
                AppRole exists = RoleManager.FindById(role.Id);

                IdentityResult result;

                if (exists == null)
                    result = await RoleManager.CreateAsync(role);
                else
                    result = await RoleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
            }
            return View(role);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            AppRole role = await RoleManager.FindByIdAsync(id);

            if (role != null && role.Users.Count == 0)
                await RoleManager.DeleteAsync(role);

            return RedirectToAction("Index");
        }
    }
}
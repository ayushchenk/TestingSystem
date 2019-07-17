using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using MailSender;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Global Admin, Education Unit Admin")]
    public class AdminController : Controller
    {
        private AdminDTO admin;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        public AdminController(IEntityService<AdminDTO> adminService,
                                 IEntityService<GroupDTO> groupService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.adminService = adminService;
            this.subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            IEnumerable<AdminDTO> admins;
            if (this.Admin.IsGlobal)
                admins = await adminService.GetAllAsync();
            else
                admins = adminService.GetAll().Where(adm => adm.EducationUnitId == this.Admin.EducationUnitId);
            if (!string.IsNullOrWhiteSpace(filter))
                return PartialView(admins.Where(user => user.Email.ToLower().Contains(filter.ToLower())
                                                                         || user.LastName.ToLower().Contains(filter.ToLower())
                                                                         || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                         || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(admins);
        }

        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await adminService.GetAsync(id);
            if (model == null || this.Admin == null)
                return RedirectToAction("Index");
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> Edit(AdminDTO model)
        {
            if (ModelState.IsValid)
            {
                AdminDTO oldUser = await adminService.GetAsync(model.Id);
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Email;
                        if(oldUser.IsGlobal != model.IsGlobal)
                        {
                            if(model.IsGlobal)
                            {
                                await UserManager.RemoveFromRoleAsync(appUser.Id, "Education Unit Admin");
                                await UserManager.AddToRoleAsync(appUser.Id, "Global Admin");
                                model.EducationUnitId = null;
                            }
                            else
                            {
                                await UserManager.RemoveFromRoleAsync(appUser.Id, "Global Admin");
                                await UserManager.AddToRoleAsync(appUser.Id, "Education Unit Admin");
                            }
                        }
                        await UserManager.UpdateAsync(appUser);
                        await adminService.AddOrUpdateAsync(model);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new AdminDTO();
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            return View(model: model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Email, Email = model.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (model.IsGlobal)
                    {
                        await UserManager.AddToRoleAsync(user.Id, "Global Admin");
                        //await UserManager.AddToRoleAsync(user.Id, "Education Unit Admin");
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(user.Id, "Education Unit Admin");
                    }
                    await adminService.AddOrUpdateAsync(model);
                    MailService sender = new MailService();
                    await sender.SendMessageAsync(model.Email, "Testing System", "Your password: " + password);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            return View(model: model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            AdminDTO user = await adminService.GetAsync(id);
            if (user != null && (this.Admin.IsGlobal || this.Admin.EducationUnitId == user.EducationUnitId))
            {
                AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var res = await UserManager.DeleteAsync(appUser);
                    if (res.Succeeded)
                    {
                        await adminService.DeleteAsync(user);
                        return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
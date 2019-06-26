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
    public class StudentController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        public StudentController(IEntityService<GroupDTO> groupService,
                                 IEntityService<StudentDTO> studentService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.studentService = studentService;
            this.subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            if (!string.IsNullOrWhiteSpace(filter))
                return PartialView(await studentService.FindByAsync(user => user.Email.ToLower().Contains(filter.ToLower())
                                                                         || user.Login.ToLower().Contains(filter.ToLower())
                                                                         || user.LastName.ToLower().Contains(filter.ToLower())
                                                                         || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                         || user.GroupName.ToLower().Contains(filter.ToLower())
                                                                         || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await studentService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await studentService.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.EducationUnitId), "Id", "GroupName", model.GroupId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StudentDTO model)
        {
            if (ModelState.IsValid)
            {
                StudentDTO oldUser = await studentService.GetAsync(model.Id);
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Login;
                        await UserManager.UpdateAsync(appUser);
                        await studentService.AddOrUpdateAsync(model);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.EducationUnitId), "Id", "GroupName", model.GroupId);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new StudentDTO();
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
            return View(model: model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(StudentDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Login, Email = model.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Student");
                    await studentService.AddOrUpdateAsync(model);
                    MailService sender = new MailService();
                    await sender.SendComplexMessageAsync(model.Email, "TestingSystem", password);
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
            ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
            return View(model: model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            StudentDTO user = await studentService.GetAsync(id);
            if (user != null)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var res = await UserManager.DeleteAsync(appUser);
                    if (res.Succeeded)
                    {
                        await studentService.DeleteAsync(user);
                        return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
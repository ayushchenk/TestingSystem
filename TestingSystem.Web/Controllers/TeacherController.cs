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
    public class TeacherController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<TeacherDTO> teacherService;
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

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        public TeacherController(IEntityService<AdminDTO> adminService,
                                 IEntityService<GroupDTO> groupService,
                                 IEntityService<TeacherDTO> teacherService,
                                 IEntityService<StudentDTO> studentService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.adminService = adminService;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            if (!string.IsNullOrWhiteSpace(filter))
                return PartialView(await teacherService.FindByAsync(user => user.Email.ToLower().Contains(filter.ToLower())
                                                                         || user.Login.ToLower().Contains(filter.ToLower())
                                                                         || user.LastName.ToLower().Contains(filter.ToLower())
                                                                         || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                         || user.SubjectName.ToLower().Contains(filter.ToLower())
                                                                         || user.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                         || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await teacherService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await teacherService.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName", model.SubjectId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                TeacherDTO oldUser = await teacherService.GetAsync(model.Id);
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Login;
                        await UserManager.UpdateAsync(appUser);
                        await teacherService.AddOrUpdateAsync(model);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName", model.SubjectId);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new TeacherDTO();
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View(model: model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Login, Email = model.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Student");
                    await teacherService.AddOrUpdateAsync(model);
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
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View(model: model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            TeacherDTO user = await teacherService.GetAsync(id);
            if (user != null)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var res = await UserManager.DeleteAsync(appUser);
                    if (res.Succeeded)
                    {
                        await teacherService.DeleteAsync(user);
                        return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
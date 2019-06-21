using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using System.Threading.Tasks;
using TestingSystem.Web.Models.ViewModels;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using MailSender;
using System.Linq;
using System.Collections.Generic;

namespace TestingSystem.Web.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class UserController : Controller
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

        public UserController(IEntityService<AdminDTO> adminService,
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
            if(!string.IsNullOrWhiteSpace(filter))
                return PartialView(await teacherService.FindByAsync(user => user.Email.ToLower().Contains(filter.ToLower()) 
                                                                      || user.Login.ToLower().Contains(filter.ToLower())
                                                                      || user.LastName.ToLower().Contains(filter.ToLower())
                                                                      || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                      || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await teacherService.GetAllAsync());
        }

        public ActionResult Create()
        {
            return View(viewName: "SelectCreate");
        }

        public async Task<ActionResult> CreateStudent()
        {
            var model = new StudentDTO();
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
            return View("Edit", model);
        }

        public async Task<ActionResult> EditStudent()
        {
            var model = new StudentDTO();
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
            return View("Edit", model);
        }

        //public async Task<ActionResult> Edit(int id = 0)
        //{
        //    var model = new EditUserViewModel();
        //    model.User = await teacherService.GetAsync(id) ?? new TeacherDTO();
        //    AppUser appUser = await UserManager.FindByEmailAsync(model.User.Email);
        //    model.Role = UserManager.GetRoles(appUser.Id).First();
        //    ViewBag.Roles = new SelectList(RoleManager.Roles, "Name", "Name", model.Role);
        //    ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.User.EducationUnitId);
        //    ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.User.EducationUnitId), "Id", "GroupName", model.User.GroupId);
        //    ViewBag.Subjects = new SelectList(subjectService.GetAll().Select
        //        (subject => new { Id = subject.Id, SubjectName = subject.SpecializationName + " - " + subject.SubjectName}), "Id", "SubjectName");
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(EditUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        TeacherDTO oldUser = await teacherService.GetAsync(model.User.Id);
        //        if (oldUser != null)
        //        {
        //            AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
        //            if (appUser != null)
        //            {
        //                await UserManager.RemoveFromRolesAsync(appUser.Id, UserManager.GetRoles(appUser.Id).ToArray());
        //                UserManager.AddToRole(appUser.Id, model.Role);
        //                appUser.Email = model.User.Email;
        //                appUser.UserName = model.User.Login;
        //                await UserManager.UpdateAsync(appUser);
        //                await teacherService.AddOrUpdateAsync(model.User);
        //                return RedirectToAction("Index");
        //            }
        //        }
        //    }
        //    ViewBag.Roles = new SelectList(RoleManager.Roles, "Name", "Name", model.Role);
        //    ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.User.EducationUnitId);
        //    ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.User.EducationUnitId), "Id", "GroupName", model.User.GroupId);
        //    ViewBag.Subjects = new SelectList(subjectService.GetAll().Select
        //        (subject => new { Id = subject.Id, SubjectName = subject.SpecializationName + " - " + subject.SubjectName }), "Id", "SubjectName", model.User.SubjectId);
        //    return View(model);
        //}

        //public async Task<ActionResult> Create()
        //{
        //    var model = new EditUserViewModel();
        //    model.User = new TeacherDTO();
        //    ViewBag.Roles = new SelectList(RoleManager.Roles, "Name", "Name");
        //    ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
        //    ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
        //    ViewBag.Subjects = new SelectList(subjectService.GetAll().Select
        //        (subject => new { Id = subject.Id, SubjectName = subject.SpecializationName + " - " + subject.SubjectName }), "Id", "SubjectName", model.User.SubjectId);
        //    return View(model: model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(EditUserViewModel model)
        //{       
        //    if (ModelState.IsValid)
        //    {
        //        AppUser user = new AppUser { UserName = model.User.Email, Email = model.User.Email };
        //        string password = Membership.GeneratePassword(10, 4);
        //        IdentityResult result = await UserManager.CreateAsync(user, password);
        //        if (result.Succeeded)
        //        {
        //            await UserManager.AddToRoleAsync(user.Id, model.Role);
        //            await teacherService.AddOrUpdateAsync(model.User);
        //            MailService sender = new MailService();
        //            await sender.SendComplexMessageAsync(model.User.Email, "TestingSystem", password);
        //            return RedirectToAction("Index", "User");
        //        }
        //        else
        //        {
        //            foreach (string error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error);
        //            }
        //        }
        //    }
        //    ViewBag.Roles = new SelectList(RoleManager.Roles, "Name", "Name", model.Role);
        //    ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.User.EducationUnitId);
        //    ViewBag.Groups = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName", model.User.GroupId);
        //    ViewBag.Subjects = new SelectList(subjectService.GetAll().Select
        //        (subject => new { Id = subject.Id, SubjectName = subject.SpecializationName + " - " + subject.SubjectName }), "Id", "SubjectName");
        //    return View(model: model);
        //}

        //public async Task<ActionResult> Delete(int id = 0)
        //{
        //    TeacherDTO user = await teacherService.GetAsync(id);
        //    if (user != null)
        //    {
        //        AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
        //        if (appUser != null)
        //        {
        //            var res = await UserManager.DeleteAsync(appUser);
        //            if (res.Succeeded)
        //            {
        //                await teacherService.DeleteAsync(user);
        //                return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
        //    }
        //    return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        //}
    }
}
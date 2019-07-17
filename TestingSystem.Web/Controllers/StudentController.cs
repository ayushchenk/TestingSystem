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
    [Authorize(Roles = "Teacher, Education Unit Admin")]
    public class StudentController : Controller
    {
        private AdminDTO admin;
        private TeacherDTO teacher;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<StudentTestResultDTO> resultService;
        private IEntityService<TeachersInGroupDTO> teacherInGroupsService;

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private TeacherDTO Teacher
        {
            get
            {
                if (teacher == null)
                    teacher = teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return teacher;
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

        private IEnumerable<int> Groups
        {
            get
            {
                return teacherInGroupsService.FindBy(tig => tig.TeacherId == this.Teacher.Id).Select(tig => tig.GroupId);
            }
        }

        public StudentController(IEntityService<GroupDTO> groupService,
                                 IEntityService<AdminDTO> adminService,
                                 IEntityService<StudentDTO> studentService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<TeacherDTO> teacherService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService,
                                 IEntityService<StudentTestResultDTO> resultService,
                                 IEntityService<TeachersInGroupDTO> teacherInGroupsService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.adminService = adminService;
            this.groupService = groupService;
            this.resultService = resultService;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.teacherInGroupsService = teacherInGroupsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            IEnumerable<StudentDTO> model = null;
            if (User.IsInRole("Teacher") && this.Teacher != null)
                model = await studentService.FindByAsync(student => this.Groups.Contains(student.GroupId));
            else if (User.IsInRole("Education Unit Admin") && this.Admin != null && this.Admin.EducationUnitId != null)
                model = await studentService.FindByAsync(student => student.EducationUnitId == this.Admin.EducationUnitId);
            else
                return RedirectToAction("Index", "Group");
            if (!string.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(user => user.Email.ToLower().Contains(filter.ToLower())
                                                                         || user.LastName.ToLower().Contains(filter.ToLower())
                                                                         || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                         || user.GroupName.ToLower().Contains(filter.ToLower())
                                                                         || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await studentService.GetAsync(id);
            if (model == null )
                return RedirectToAction("Index");
            if (User.IsInRole("Teacher") && this.Groups.Contains(model.GroupId))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Groups.Contains(group.Id)), "Id", "GroupName", model.GroupId);
            else if (User.IsInRole("Education Unit Admin") && this.Admin.EducationUnitId == model.EducationUnitId)
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Admin.EducationUnitId == group.EducationUnitId), "Id", "GroupName", model.GroupId);
            else
                return RedirectToAction("Index");
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
                        appUser.UserName = model.Email;
                        await UserManager.UpdateAsync(appUser);
                        await studentService.AddOrUpdateAsync(model);
                        return RedirectToAction("Index");
                    }
                }
            }
            if(User.IsInRole("Teacher"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Groups.Contains(group.Id)), "Id", "GroupName");
            else if (User.IsInRole("Education Unit Admin"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Admin.EducationUnitId == group.EducationUnitId), "Id", "GroupName");
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            if (this.Teacher == null && this.Admin == null)
                return RedirectToAction("Index");
            var model = new StudentDTO();
            if (User.IsInRole("Teacher"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Groups.Contains(group.Id)), "Id", "GroupName");
            else if (User.IsInRole("Education Unit Admin"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Admin.EducationUnitId == group.EducationUnitId), "Id", "GroupName");
            return View(model: model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(StudentDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Email, Email = model.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    model.EducationUnitId = (this.Teacher == null) ? this.Admin.EducationUnitId.Value : this.Teacher.EducationUnitId;
                    await UserManager.AddToRoleAsync(user.Id, "Student");
                    await studentService.AddOrUpdateAsync(model);
                    MailService sender = new MailService();
                    await sender.SendMessageAsync(model.Email, "TestingSystem", "Your password: " + password);
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
            if(User.IsInRole("Teacher"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Groups.Contains(group.Id)), "Id", "GroupName");
            else if (User.IsInRole("Education Unit Admin"))
                ViewBag.Groups = new SelectList(await groupService.FindByAsync(group => this.Admin.EducationUnitId == group.EducationUnitId), "Id", "GroupName");
            return View(model: model);
        }

        public async Task<ActionResult> History(int id = 0)
        {
            StudentDTO student = await studentService.GetAsync(id);
            if (student == null)
                return RedirectToAction("Index");
            ViewBag.Student = student;
            var model = await resultService.GetAllAsync();
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            StudentDTO user = await studentService.GetAsync(id);
            if ((user != null && this.Teacher != null && this.Groups.Contains(user.GroupId)) ||
               (this.Admin != null && user.EducationUnitId == this.Admin.EducationUnitId))
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
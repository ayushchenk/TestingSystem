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
    [Authorize(Roles = "Education Unit Admin, Global Admin")]
    public class TeacherController : Controller
    {
        private AdminDTO admin;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<TeachersInGroupDTO> teacherInGroupsService;
        private IEntityService<TeachersInSubjectDTO> teacherInSubjectsService;

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

        public TeacherController(IEntityService<AdminDTO> adminService,
                                 IEntityService<GroupDTO> groupService,
                                 IEntityService<TeacherDTO> teacherService,
                                 IEntityService<StudentDTO> studentService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService,
                                 IEntityService<TeachersInGroupDTO> teacherInGroupsService,
                                 IEntityService<TeachersInSubjectDTO> teacherInSubjectsService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.adminService = adminService;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.teacherInGroupsService = teacherInGroupsService;
            this.teacherInSubjectsService = teacherInSubjectsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialIndex(string filter = null)
        {
            IEnumerable<TeacherDTO> teachers;
            if (this.Admin.IsGlobal)
                teachers = teacherService.GetAll().ToList();
            else
                teachers = teacherService.FindBy(teacher => teacher.EducationUnitId == this.Admin.EducationUnitId).ToList();
            foreach (var teacher in teachers)
            {
                teacher.Subjects = new List<SubjectDTO>();
                var subjectIds = teacherInSubjectsService.FindBy(tis => tis.TeacherId == teacher.Id).Select(tis => tis.SubjectId).ToList();
                foreach (var s in subjectService.FindBy(subject => subjectIds.Contains(subject.Id)).ToList())
                    teacher.Subjects.Add(s);
            }
            ViewBag.DeletedTeachers = teachers.Where(teacher => teacher.IsDeleted);
            teachers = teachers.Where(teacher => !teacher.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter))
                teachers = teachers.Where(user => user.Email.ToLower().Contains(filter.ToLower())
                                                                         || user.LastName.ToLower().Contains(filter.ToLower())
                                                                         || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                         || user.Subjects.Select(subject => subject.SubjectName.ToLower()).Contains(filter.ToLower())
                                                                         || user.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                         || user.EducationUnitName.ToLower().Contains(filter.ToLower()));
            return PartialView(teachers);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await teacherService.GetAsync(id);
            if (model == null || this.Admin == null || model.EducationUnitId != this.Admin.EducationUnitId)
                return RedirectToAction("Index");
            model.SubjectId = 1;
            model.Subjects = new List<SubjectDTO>();
            var subjectIds = teacherInSubjectsService.FindBy(tis => tis.TeacherId == model.Id).Select(tis => tis.SubjectId).ToList();
            foreach (var s in subjectService.FindBy(subject => subjectIds.Contains(subject.Id)).ToList())
                model.Subjects.Add(s);
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                await teacherService.AddOrUpdateAsync(model);
                return RedirectToAction("Index");
            }
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            model.Subjects = new List<SubjectDTO>();
            var subjectIds = teacherInSubjectsService.FindBy(tis => tis.TeacherId == model.Id).Select(tis => tis.SubjectId).ToList();
            foreach (var s in subjectService.FindBy(subject => subjectIds.Contains(subject.Id)).ToList())
                model.Subjects.Add(s);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View(model: new TeacherDTO());
        }

        [HttpPost]
        public async Task<ActionResult> Create(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                if (!this.Admin.IsGlobal)
                    model.EducationUnitId = this.Admin.EducationUnitId.Value;
                AppUser user = new AppUser { UserName = model.Email, Email = model.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    int subId = model.SubjectId;
                    await UserManager.AddToRoleAsync(user.Id, "Teacher");
                    model = await teacherService.AddOrUpdateAsync(model);
                    await teacherInSubjectsService.AddOrUpdateAsync(new TeachersInSubjectDTO() { TeacherId = model.Id, SubjectId = subId });
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
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View(model: model);
        }

        public async Task<ActionResult> AddSubject(int id = 0)
        {
            var item = await teacherService.GetAsync(id);
            if (item == null)
                return RedirectToAction("Index");
            var subjectIds = teacherInSubjectsService.FindBy(tis => tis.TeacherId == item.Id).Select(tis => tis.SubjectId).ToList();
            ViewBag.Subjects = await subjectService.FindByAsync(subject => subject.SpecializationId == item.SpecializationId && !subjectIds.Contains(subject.Id));
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubject(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                await teacherInSubjectsService.AddOrUpdateAsync(new TeachersInSubjectDTO { TeacherId = model.Id, SubjectId = model.SubjectId });
                return RedirectToAction("Index");
            }
            var subjectIds = teacherInSubjectsService.FindBy(tis => tis.TeacherId == model.Id).Select(tis => tis.SubjectId).ToList();
            ViewBag.Subjects = await subjectService.FindByAsync(subject => subject.SpecializationId == model.SpecializationId && !subjectIds.Contains(subject.Id));
            return View(model);
        }

        public async Task<ActionResult> Restore(int id = 0)
        {
            var item = await teacherService.GetAsync(id);
            if (item != null)
            {
                item.IsDeleted = false;
                await teacherService.AddOrUpdateAsync(item);
                return Json($"Successfully restored teacher: #{item.Id} - \"{item.Email}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await teacherService.GetAsync(id);
            if (item != null)
            {
                item.IsDeleted = true;
                await teacherService.AddOrUpdateAsync(item);
                return Json($"Successfully archived teacher: #{item.Id} - \"{item.Email}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeleteForever(int id = 0)
        {
            TeacherDTO user = await teacherService.GetAsync(id);
            if (user != null && (this.Admin.IsGlobal || this.Admin.EducationUnitId == user.EducationUnitId))
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
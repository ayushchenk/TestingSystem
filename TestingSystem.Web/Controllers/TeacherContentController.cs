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
    [Authorize(Roles = "Teacher")]
    public class TeacherContentController : Controller
    {
        private TeacherDTO teacher;
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<GroupsInTestDTO> gitService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<StudentTestResultDTO> resultService;
        private IEntityService<TeachersInGroupDTO> teachersInGroupsService;
        private IEntityService<TeachersInSubjectDTO> teachersInSubejctsService;

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

        public TeacherContentController(IEntityService<TestDTO> testService,
                                        IEntityService<GroupDTO> groupService,
                                        IEntityService<TeacherDTO> teacherService,
                                        IEntityService<StudentDTO> studentService,
                                        IEntityService<GroupsInTestDTO> gitService,
                                        IEntityService<QuestionDTO> questionService,
                                        IEntityService<QuestionAnswerDTO> answerService,
                                        IEntityService<StudentTestResultDTO> resultService,
                                        IEntityService<TeachersInGroupDTO> teachersInGroupsService,
                                        IEntityService<TeachersInSubjectDTO> teachersInSubejctsService)
        {
            this.gitService = gitService;
            this.testService = testService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.resultService = resultService;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.questionService = questionService;
            this.teachersInGroupsService = teachersInGroupsService;
            this.teachersInSubejctsService = teachersInSubejctsService;
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public async Task<ActionResult> Groups()
        {
            List<TeacherGroupSubjects> model = new List<TeacherGroupSubjects>();
            var groups = teachersInGroupsService.FindBy(tig => tig.TeacherId == this.Teacher.Id);
            var groupIds = groups.Select(tig => tig.GroupId);
            foreach (var group in await groupService.FindByAsync(group => groupIds.Contains(group.Id)))
                model.Add(new TeacherGroupSubjects
                {
                    Group = group,
                    Subjects = groups.Where(tig => tig.TeacherId == this.Teacher.Id && tig.GroupId == group.Id)
                });
            return View(model);
        }

        public async Task<ActionResult> Students(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Groups");
            ViewBag.GroupName = group.GroupName;
            var model = await studentService.FindByAsync(student => student.GroupId == group.Id);
            return View(model);
        }

        public new async Task<ActionResult> Profile()
        {
            if (this.Teacher == null)
                return RedirectToAction("Groups");
            var appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (appUser == null)
                return RedirectToAction("Groups");
            var model = new EditTeacherViewModel
            {
                Teacher = this.Teacher,
                IsTwoFactorEnabled = appUser.TwoFactorEnabled
            };
            return View(model);
        }

        public async Task<ActionResult> Edit()
        {
            if (this.Teacher == null)
                return RedirectToAction("Groups");
            var appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (appUser == null)
                return RedirectToAction("Groups");
            var model = new EditTeacherViewModel
            {
                Teacher = this.Teacher,
                IsTwoFactorEnabled = appUser.TwoFactorEnabled
            };
            model.Teacher.SubjectId = int.MaxValue;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(User.Identity.Name);
                if (appUser != null)
                {
                    appUser.Email = model.Teacher.Email;
                    appUser.UserName = model.Teacher.Email;
                    appUser.TwoFactorEnabled = model.IsTwoFactorEnabled;
                    await UserManager.UpdateAsync(appUser);
                    await teacherService.AddOrUpdateAsync(model.Teacher);
                }
                return RedirectToAction("Profile");
            }
            return View(model);
        }
    }
}
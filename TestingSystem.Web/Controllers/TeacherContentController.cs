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
                if(teacher == null)
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
                                        IEntityService<TeachersInGroupDTO> teachersInGroupsService)
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
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public async Task<ActionResult> Groups()
        {
            var groups = teachersInGroupsService.FindBy(tig => tig.TeacherId == this.Teacher.Id).Select(tig => tig.GroupId);
            var model = await groupService.FindByAsync(group => groups.Contains(group.Id));
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

        public new ActionResult Profile()
        {
            if (this.Teacher == null)
                return RedirectToAction("Groups");
            return View(this.Teacher);
        }

        public ActionResult Edit()
        {
            if (this.Teacher == null)
                return RedirectToAction("Groups");
            return View(this.Teacher);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                TeacherDTO oldUser = this.Teacher;
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Email;
                        await UserManager.UpdateAsync(appUser);
                        await teacherService.AddOrUpdateAsync(model);
                        return RedirectToAction("Profile");
                    }
                }
            }
            return View(model);
        }
    }
}
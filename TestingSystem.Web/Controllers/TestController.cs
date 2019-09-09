using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TestController : Controller
    {
        private TeacherDTO teacher;
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;
        private IEntityService<TeachersInGroupDTO> teachersInGroupsService;
        private IEntityService<TeachersInSubjectDTO> teachersInSubjectsService;

        private TeacherDTO Teacher
        {
            get
            {
                if (teacher == null)
                    teacher = teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return teacher;
            }
        }

        public TestController(IEntityService<TestDTO> testService, 
                              IEntityService<GroupDTO> groupService,
                              IEntityService<TeacherDTO> teacherService,
                              IEntityService<SubjectDTO> subjectService,
                              IEntityService<QuestionDTO> questionService,
                              IEntityService<SpecializationDTO> specService, 
                              IEntityService<QuestionAnswerDTO> answerService,
                              IEntityService<GroupsInTestDTO> groupsInTestService,
                              IEntityService<TeachersInGroupDTO> teachersInGroupsService,
                              IEntityService<TeachersInSubjectDTO> teachersInSubjectsService)
        {
            this.testService = testService;
            this.specService = specService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.questionService = questionService;
            this.groupsInTestService = groupsInTestService;
            this.teachersInGroupsService = teachersInGroupsService;
            this.teachersInSubjectsService = teachersInSubjectsService;
        }

        public ActionResult Index()
        {
            if (Teacher == null)
                return RedirectToRoute("TeacherContent");
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            var model = await testService.FindByAsync(test => test.TeacherId == this.Teacher.Id);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(test => test.TestName.ToLower().Contains(filter.ToLower())
                                                                      || test.SpecializationName.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public async Task<ActionResult> Create()
        {
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id)), "Id", "SubjectName");
            return View("Edit", new TestDTO() { TeacherId = this.Teacher.Id});
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await testService.GetAsync(id);
            if (model == null || model.TeacherId != this.Teacher.Id)
                return RedirectToAction("Index");
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id)), "Id", "SubjectName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TestDTO model)
        {
            if (ModelState.IsValid)
            {
                await testService.AddOrUpdateAsync(model);
                return RedirectToAction("Index");
            }
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id)), "Id", "SubjectName");
            return View(model);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && item.TeacherId == this.Teacher.Id)
            {
                var groups = await groupsInTestService.FindByAsync(git => git.TestId == item.Id);
                if (groups.Count() != 0)
                    return Json($"This test is currently in use: Id = {item.Id} - TestName = {item.TestName}", JsonRequestBehavior.AllowGet);
                await testService.DeleteAsync(item);
                return Json($"Successfully deleted item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> QuickTestSetup()
        {
            var model = new QuickTestSetupViewModel()
            {
                Specializations = await specService.GetAllAsync(),
                Subjects = await subjectService.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> QuickTestSetup(QuickTestSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var questions = await questionService.FindByAsync(q => q.SubjectId == model.SubjectId);
                var questionIds = questions.Select(q => q.Id);
                var allAnswers = await answerService.FindByAsync(a => questionIds.Contains(a.QuestionId));
                QuickTestCheckViewModel testViewModel = new QuickTestCheckViewModel()
                {
                    SubjectId = model.SubjectId,
                    SpecializationId = model.SpecializationId,
                    QuestionCount = model.QuestionCount
                };
                Random rnd = new Random();
                int realCount = Math.Min(model.QuestionCount, questions.Count());
                for (int i = 0; i < model.QuestionCount; i++)
                {
                    int selected = rnd.Next(realCount);
                    testViewModel.QuestionAnswers.Add(new QuestionAnswer
                    {
                        Question = questions.ElementAt(selected),
                        Answers = allAnswers.Where(ans => ans.QuestionId == questions.ElementAt(selected).Id).ToList()
                    });
                }
                model.Specializations = await specService.GetAllAsync();
                model.Subjects = await subjectService.GetAllAsync();
                return View("QuickTest", testViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult QuickTestCheck(QuickTestCheckViewModel model)
        {
             return View("QuickTestResult", model);
        }
    }
}
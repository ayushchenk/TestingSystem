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
        private IEntityService<SubjectThemeDTO> themeService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;
        private IEntityService<ThemesInTestDTO> themesInTestsService;
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
                              IEntityService<SubjectThemeDTO> themeService,
                              IEntityService<SpecializationDTO> specService,
                              IEntityService<QuestionAnswerDTO> answerService,
                              IEntityService<GroupsInTestDTO> groupsInTestService,
                              IEntityService<ThemesInTestDTO> themesInTestsService,
                              IEntityService<TeachersInGroupDTO> teachersInGroupsService,
                              IEntityService<TeachersInSubjectDTO> teachersInSubjectsService)
        {
            this.testService = testService;
            this.specService = specService;
            this.groupService = groupService;
            this.themeService = themeService;
            this.answerService = answerService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.questionService = questionService;
            this.groupsInTestService = groupsInTestService;
            this.themesInTestsService = themesInTestsService;
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
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            var model = (await testService.FindByAsync(test => test.TeacherId == this.Teacher.Id && ids.Contains(test.SubjectId) && !test.IsDeleted)).ToList();
            foreach (var test in model)
                test.Themes = (await themesInTestsService.FindByAsync(tit => tit.TestId == test.Id)).ToList();


            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(test => test.TestName.ToLower().Contains(filter.ToLower())
                                                                      || test.SpecializationName.ToLower().Contains(filter.ToLower())));
            ViewBag.DeletedTests = await testService.FindByAsync(test => test.TeacherId == this.Teacher.Id && ids.Contains(test.SubjectId) && test.IsDeleted);
            return PartialView(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new CreateTestViewModel()
            {
                Test = new TestDTO() { TeacherId = this.Teacher.Id },
                SelectThemeItems = new List<SelectListItem>(),
            };
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id) && subject.Questions > 0), "Id", "SubjectName"); ;
            return View(model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var test = await testService.GetAsync(id);
            if (test == null || test.TeacherId != this.Teacher.Id)
                return RedirectToAction("Index");

            var model = new CreateTestViewModel()
            {
                Test = test,
                SelectedThemes = (await themesInTestsService.FindByAsync(tit => tit.TestId == test.Id)).Select(tit => tit.ThemeId),
                Themes = (await themeService.FindByAsync(theme => theme.TeacherId == this.Teacher.Id && theme.Questions > 0 && theme.SubjectId == test.SubjectId)).ToList(),
                SelectThemeItems = (await themeService.FindByAsync(theme => theme.TeacherId == this.Teacher.Id && theme.Questions > 0 && theme.SubjectId == test.SubjectId)).Select(theme => new SelectListItem() { Text = theme.ThemeName, Value = theme.Id.ToString() }).ToList()
            };

            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id) && subject.Questions > 0), "Id", "SubjectName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldThemes = await themesInTestsService.FindByAsync(tit => tit.TestId == model.Test.Id);
                await themesInTestsService.DeleteRangeAsync(oldThemes);

                model.Test = await testService.AddOrUpdateAsync(model.Test);
                foreach (var theme in model.SelectedThemes)
                    await themesInTestsService.AddOrUpdateAsync(new ThemesInTestDTO { ThemeId = theme, TestId = model.Test.Id });
                return RedirectToAction("Index");
            }
            var ids = (await teachersInSubjectsService.FindByAsync(tis => tis.TeacherId == this.Teacher.Id)).Select(tis => tis.SubjectId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => ids.Contains(subject.Id)), "Id", "SubjectName");
            model.Test = await testService.GetAsync(model.Test.Id);
            model.SelectedThemes = (await themesInTestsService.FindByAsync(tit => tit.TestId == model.Test.Id)).Select(tit => tit.ThemeId);
            model.Themes = (await themeService.FindByAsync(theme => theme.TeacherId == this.Teacher.Id && theme.Questions > 0 && theme.SubjectId == model.Test.SubjectId)).ToList();
            model.SelectThemeItems = (await themeService.FindByAsync(theme => theme.TeacherId == this.Teacher.Id && theme.Questions > 0 && theme.SubjectId == model.Test.SubjectId)).Select(theme => new SelectListItem() { Text = theme.ThemeName, Value = theme.Id.ToString() }).ToList();
            return View(model);
        }

        public async Task<JsonResult> Restore(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                item.IsDeleted = false;
                await testService.AddOrUpdateAsync(item);
                return Json($"Successfully restored item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && item.TeacherId == this.Teacher.Id)
            {
                item.IsDeleted = true;
                await testService.AddOrUpdateAsync(item);
                return Json($"Successfully archived item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteForever(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && item.TeacherId == this.Teacher.Id && item.IsDeleted)
            {
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
                model.Subjects = null;
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
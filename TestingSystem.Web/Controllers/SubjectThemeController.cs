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
    public class SubjectThemeController : Controller
    {
        private TeacherDTO teacher;
        private IEntityService<TestDTO> testService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SubjectThemeDTO> themeService;
        private IEntityService<ThemesInTestDTO> themeInTestService;

        private TeacherDTO Teacher
        {
            get
            {
                if (teacher == null)
                    teacher = teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return teacher;
            }
        }

        public SubjectThemeController(IEntityService<TestDTO> testService,
                                      IEntityService<TeacherDTO> teacherService,
                                      IEntityService<SubjectDTO> subjectService,
                                      IEntityService<QuestionDTO> questionService,
                                      IEntityService<SubjectThemeDTO> themeService,
                                      IEntityService<SpecializationDTO> specService,
                                      IEntityService<ThemesInTestDTO> themeInTestService)
        {
            this.testService = testService;
            this.themeService = themeService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.questionService = questionService;
            this.themeInTestService = themeInTestService;
        }

        public async Task<ActionResult> Index(int id = 0)
        {
            var model = await subjectService.GetAsync(id);
            if (model == null)
                return RedirectToAction("Subjects", "TeacherContent");
            return View(model);
        }

        public async Task<PartialViewResult> PartialIndex(int id = 0, string filter = null)
        {
            var model = await themeService.FindByAsync(theme => theme.SubjectId == id);
            ViewBag.SubjectId = id;
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(theme => theme.ThemeName.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public async Task<ActionResult> Create(int id = 0)
        {
            var subject = await subjectService.GetAsync(id);
            if (subject == null)
                return RedirectToAction("Subjects", "TeacherContent");
            var model = new SubjectThemeDTO()
            {
                Id = 0,
                SubjectId = subject.Id,
                SubjectName = subject.SubjectName,
                TeacherId = this.Teacher.Id
            };
            return View("Edit", model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await themeService.GetAsync(id);
            if (this.Teacher == null || model == null || model.TeacherId != this.Teacher.Id)
                return RedirectToAction("Subjects", "TeacherContent");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SubjectThemeDTO model)
        {
            if (ModelState.IsValid)
            {
                await themeService.AddOrUpdateAsync(model);
                return RedirectToAction("Index", new { id = model.SubjectId });
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await themeService.GetAsync(id);
            if (item != null && this.Teacher != null && item.TeacherId == this.Teacher.Id)
            {
                var tests = await themeInTestService.FindByAsync(test => test.ThemeId == item.Id);
                var questions = await questionService.FindByAsync(question => question.ThemeId == item.Id);
                if (questions.Count() != 0)
                    return Json($"There are questions relying on such theme: Id = {item.Id}, ThemeName = {item.ThemeName}", JsonRequestBehavior.AllowGet);
                if (tests.Count() != 0)
                    return Json($"There are tests relying on such theme: Id = {item.Id}, SubjectName = {item.ThemeName}", JsonRequestBehavior.AllowGet);
                await themeService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.ThemeName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
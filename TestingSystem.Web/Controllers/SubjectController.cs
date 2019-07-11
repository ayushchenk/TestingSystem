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
    [Authorize(Roles = "Education Unit Admin, Global Admin")]
    public class SubjectController : Controller
    {
        private AdminDTO admin;
        private IEntityService<TestDTO> testService;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SpecializationDTO> specService;

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        public SubjectController(IEntityService<TestDTO> testService,
                                 IEntityService<AdminDTO> adminService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<QuestionDTO> questionService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.testService = testService;
            this.specService = specService;
            this.adminService = adminService;
            this.subjectService = subjectService;
            this.questionService = questionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            ViewBag.IsGlobal = this.Admin?.IsGlobal ?? false;
            ViewBag.EducationUnitId = this.Admin?.EducationUnitId;
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await subjectService.FindByAsync(subject => subject.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                            || subject.SubjectName.ToLower().Contains(filter.ToLower())));
            return PartialView(await subjectService.GetAllAsync());
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            return View("Edit", new SubjectDTO());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await subjectService.GetAsync(id);
            if (this.Admin == null || model == null || ((model.EducationUnitId ?? 0) != (this.Admin.EducationUnitId ?? 0) && !this.Admin.IsGlobal))
                return RedirectToAction("Index");
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SubjectDTO subject)
        {
            if (ModelState.IsValid)
            {
                subject.EducationUnitId = this.Admin.EducationUnitId;
                await subjectService.AddOrUpdateAsync(subject);
                return RedirectToAction("Index");
            }
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", subject.SpecializationId);
            return View(subject);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await subjectService.GetAsync(id);
            if (item != null && this.Admin != null && ((item.EducationUnitId ?? 0) == (this.Admin.EducationUnitId ?? 0) || this.Admin.IsGlobal))
            {
                var tests = await testService.FindByAsync(test => test.SubjectId == item.Id);
                var questions = await questionService.FindByAsync(question => question.SubjectId == item.Id);
                if (questions.Count() != 0)
                    return Json($"There are questions relying on such subject: Id = {item.Id}, SubjectName = {item.SubjectName}", JsonRequestBehavior.AllowGet);
                if (tests.Count() != 0)
                    return Json($"There are tests relying on such subject: Id = {item.Id}, SubjectName = {item.SubjectName}", JsonRequestBehavior.AllowGet);
                await subjectService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.SubjectName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
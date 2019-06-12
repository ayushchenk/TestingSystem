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
    public class SubjectController : Controller
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SpecializationDTO> specService;

        public SubjectController(IEntityService<TestDTO> testService, 
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<QuestionDTO> questionService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.testService = testService;
            this.specService = specService;
            this.subjectService = subjectService;
            this.questionService = questionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await subjectService.FindByAsync(subject => subject.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                            || subject.SubjectName.ToLower().Contains(filter.ToLower())));
            return PartialView(await subjectService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await subjectService.GetAsync(id) ?? new SubjectDTO();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SubjectDTO subject)
        {
            if (ModelState.IsValid)
            {
                await subjectService.AddOrUpdateAsync(subject);
                return RedirectToAction("Index");
            }
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", subject.SpecializationId);
            return View(subject);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await subjectService.GetAsync(id);
            if (item != null)
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
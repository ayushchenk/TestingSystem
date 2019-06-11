using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;
using System.Linq;

namespace TestingSystem.Web.Controllers
{
    public class QuestionController : Controller
    {
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionImageDTO> imageService;
        private IEntityService<QuestionAnswerDTO> answerService;

        public QuestionController(IEntityService<QuestionDTO> questionService,
                                  IEntityService<SpecializationDTO> specService,
                                  IEntityService<QuestionImageDTO> imageService,
                                  IEntityService<QuestionAnswerDTO> answerService,
                                  IEntityService<SubjectDTO> subjectService)
        {
            this.specService = specService;
            this.imageService = imageService;
            this.answerService = answerService;
            this.subjectService = subjectService;
            this.questionService = questionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            var model = new List<CreateQuestionViewModel>();
            IEnumerable<QuestionDTO> questions;
            if (String.IsNullOrWhiteSpace(filter))
                questions = await questionService.GetAllAsync();
            else
                questions = await questionService.FindByAsync(question => question.SubjectName.ToLower().Contains(filter.ToLower())
                                                                       || question.SpecializationName.ToLower().Contains(filter.ToLower()));
            foreach (var q in questions)
                model.Add(new CreateQuestionViewModel
                {
                    Question = q,
                    Answers = answerService.FindBy(answer => answer.QuestionId == q.Id).ToList()
                });
            return PartialView(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new CreateQuestionViewModel();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var upload = Request.Files[0];
                    string fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("~/Images/" + fileName));
                    model.Question.ImagePath = @"/Images/" + fileName;
                    var saved = await imageService.AddOrUpdateAsync(new QuestionImageDTO
                    {
                        ImagePath = model.Question.ImagePath,
                    });
                    model.Question.QuestionImageId = saved.Id;
                }

                model.Question = await questionService.AddOrUpdateAsync(model.Question);
                foreach (var answer in model.Answers.Where(a => !String.IsNullOrWhiteSpace(a.AnswerString)))
                {
                    answer.QuestionId = model.Question.Id;
                    await answerService.AddOrUpdateAsync(answer);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = new CreateQuestionViewModel();
            model.Question = await questionService.GetAsync(id) ?? new QuestionDTO();
            model.Answers = answerService.FindBy(answer => answer.QuestionId == model.Question.Id).ToList();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Question.SpecializationId);
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName", model.Question.SubjectId);
            return View("Create", model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await questionService.GetAsync(id);
            if (item != null)
            {
                //await answerService.DeleteRangeAsync(answer => answer.QuestionId == item.Id);
                await questionService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.SpecializationName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
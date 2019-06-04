using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.Controllers
{
    public class QuestionController : Controller
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionImageDTO> imageService;

        public QuestionController(IEntityService<TestDTO> testService, IEntityService<QuestionDTO> questionService, IEntityService<SpecializationDTO> specService, IEntityService<QuestionImageDTO> imageService)
        {
            this.testService = testService;
            this.specService = specService;
            this.imageService = imageService;
            this.questionService = questionService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", 0);
            return View();
        }

        public async Task<ActionResult> PartialIndex(int filter = 0)
        {
            ViewBag.SpecializationId = filter;
            if(filter != 0)
                return PartialView(await questionService.FindByAsync(question => question.SpecializationId == filter));
            return PartialView(await questionService.GetAllAsync());
        }

        public async Task<ActionResult> Create(int specId = 0)
        {
            var model = new CreateQuestionViewModel();
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", specId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var upload = Request.Files[0];
                    string fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("~/Images/" + fileName));
                    model.Question.ImagePath = @"/Images/" + fileName;
                    await imageService.AddOrUpdateAsync(new QuestionImageDTO
                    {
                        ImagePath = model.Question.ImagePath
                    });
                }

                await questionService.AddOrUpdateAsync(model.Question);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int questionId = 0)
        {
            var model = await questionService.GetAsync(questionId) ?? new QuestionDTO();
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(QuestionDTO model)
        {
            //if (ModelState.IsValid)
            //{
            //    if (Request.Files.Count > 0)
            //    {
            //        var upload = Request.Files[0];
            //        string fileName = DateTime.Now.Millisecond + System.IO.Path.GetFileName(upload.FileName);
            //        upload.SaveAs(Server.MapPath("~/Images/" + fileName));
            //        model.ImagePath = @"/Images/" + fileName;
            //    }

            //    await questionService.AddOrUpdateAsync(model);

            //    return RedirectToAction("Index");
            //}
            //return View(model);
            return new EmptyResult();
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await questionService.GetAsync(id);
            if (item != null)
            {
                await questionService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.SpecializationName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
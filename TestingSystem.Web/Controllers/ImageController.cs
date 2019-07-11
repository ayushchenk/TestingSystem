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
    [Authorize(Roles = "Teacher, Education Unit Admin, Global Admin")]
    public class ImageController : Controller
    {
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionImageDTO> imageService;

        public ImageController(IEntityService<QuestionDTO> questionService,
                               IEntityService<QuestionImageDTO> imageService)
        {
            this.imageService = imageService;
            this.questionService = questionService;
        }

        public ActionResult Index() => View();

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await imageService.FindByAsync(image => image.ImagePath.ToLower().Contains(filter.ToLower())));
            return PartialView(await imageService.GetAllAsync());
        }

        public ActionResult Create() => View(new QuestionImageDTO());

        [HttpPost]
        public async Task<ActionResult> Create(QuestionImageDTO image)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files[0].ContentLength > 0)
                {
                    var upload = Request.Files[0];
                    string fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("~/Images/" + fileName));
                    image.ImagePath = @"/Images/" + fileName;
                    await imageService.AddOrUpdateAsync(image);
                }
                return RedirectToAction("Index");
            }
            return View(image);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await imageService.GetAsync(id);
            if (item != null)
            {
                var questions = await questionService.FindByAsync(question => question.QuestionImageId == item.Id);
                if (questions.Count() != 0)
                    return Json($"There are questions relying on such image: Id = {item.Id}", JsonRequestBehavior.AllowGet);
                System.IO.File.Delete(Server.MapPath(item.ImagePath));
                await imageService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
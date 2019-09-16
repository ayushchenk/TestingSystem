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
    public class ImageController : Controller
    {
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionImageDTO> imageService;

        private TeacherDTO Teacher
        {
            get
            {
                return teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
            }
        }

        public ImageController(IEntityService<QuestionDTO> questionService,
                               IEntityService<TeacherDTO> teacherService,
                               IEntityService<QuestionImageDTO> imageService)
        {
            this.imageService = imageService;
            this.teacherService = teacherService;
            this.questionService = questionService;
        }

        public ActionResult Index() => View();

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            var model = await imageService.FindByAsync(image=> image.TeacherId == this.Teacher.Id);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(image => image.ImagePath.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public ActionResult Create() => View(new QuestionImageDTO());

        [HttpPost]
        public async Task<ActionResult> Create(QuestionImageDTO image)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files[0].ContentLength > 0)
                {
                    if (this.Teacher != null)
                        image.TeacherId = this.Teacher.Id;
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
            if (item != null && this.Teacher != null && item.TeacherId == this.Teacher.Id)
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
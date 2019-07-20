using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestingSystem.BusinessModel.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Teacher, Education Unit Admin, Global Admin")]
    public class ImageController : Controller
    {
        private IEntityService<AdminDTO> adminService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionImageDTO> imageService;

        private AdminDTO Admin
        {
            get
            {
                return adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
            }
        }

        private TeacherDTO Teacher
        {
            get
            {
                return teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
            }
        }

        public ImageController(IEntityService<QuestionDTO> questionService,
                               IEntityService<AdminDTO> adminService,
                               IEntityService<TeacherDTO> teacherService,
                               IEntityService<QuestionImageDTO> imageService)
        {
            this.adminService = adminService;
            this.imageService = imageService;
            this.teacherService = teacherService;
            this.questionService = questionService;
        }

        public ActionResult Index() => View();

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            ViewBag.IsGlobal = this.Admin?.IsGlobal ?? false;
            ViewBag.EducationUnitId = this.Admin?.EducationUnitId;
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
                    if (this.Admin != null && !this.Admin.IsGlobal)
                        image.EducationUnitId = this.Admin.EducationUnitId;
                    if (this.Teacher != null)
                        image.EducationUnitId = this.Teacher.EducationUnitId;
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
            if (item != null && ((this.Teacher != null && item.EducationUnitId == this.Teacher.EducationUnitId) ||
                (this.Admin != null && (item.EducationUnitId == this.Admin.EducationUnitId || this.Admin.IsGlobal))))
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
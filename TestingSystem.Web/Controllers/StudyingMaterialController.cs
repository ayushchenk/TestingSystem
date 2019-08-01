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
    public class StudyingMaterialController : Controller
    {
        private TeacherDTO teacher;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<StudyingMaterialDTO> materialService;

        private TeacherDTO Teacher
        {
            get
            {
                if (teacher == null)
                    teacher = teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return teacher;
            }
        }

        public StudyingMaterialController(IEntityService<TeacherDTO> teacherService,
                                          IEntityService<SubjectDTO> subjectService,
                                          IEntityService<StudyingMaterialDTO> materialService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.materialService = materialService;
        }

        public ActionResult Index()
        {
            if (Teacher == null)
                return RedirectToRoute("TeacherContent");
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            var model = await materialService.FindByAsync(material => material.TeacherId == this.Teacher.Id);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(test => test.StudyingMaterialName.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public ActionResult Create()
        {
            return View("Edit", new StudyingMaterialDTO());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await materialService.GetAsync(id);
            if (model == null || model.TeacherId != this.Teacher.Id)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StudyingMaterialDTO model)
        {
            if (ModelState.IsValid)
            {
                model.TeacherId = this.Teacher.Id;
                await materialService.AddOrUpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await materialService.GetAsync(id);
            if (item != null && item.TeacherId == this.Teacher.Id)
            {
                await materialService.DeleteAsync(item);
                return Json($"Successfully deleted item: #{item.Id} - \"{item.StudyingMaterialName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
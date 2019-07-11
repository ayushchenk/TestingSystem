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
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;

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
                              IEntityService<SpecializationDTO> specService, 
                              IEntityService<GroupsInTestDTO> groupsInTestService)
        {
            this.testService = testService;
            this.specService = specService;
            this.groupService = groupService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.groupsInTestService = groupsInTestService;
        }

        public ActionResult Index()
        {
            if (Teacher == null)
                return RedirectToRoute("TeacherContent");
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            var model = await testService.FindByAsync(test => test.TeacherId == this.Teacher.Id);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(model.Where(test => test.TestName.ToLower().Contains(filter.ToLower())
                                                                      || test.SpecializationName.ToLower().Contains(filter.ToLower())));
            return PartialView(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new TestDTO();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.Subjects = new SelectList(await subjectService.GetAllAsync(), "Id", "SubjectName");
            return View("Edit", model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await testService.GetAsync(id);
            if (model == null || model.EducationUnitId != this.Teacher.EducationUnitId)
                return RedirectToAction("Index");
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => subject.SpecializationId == model.SpecializationId), "Id", "SubjectName", model.SubjectId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TestDTO model)
        {
            if (ModelState.IsValid)
            {
                model.TeacherId = this.Teacher.Id;
                await testService.AddOrUpdateAsync(model);
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            ViewBag.Subjects = new SelectList(await subjectService.FindByAsync(subject => subject.SpecializationId == model.SpecializationId), "Id", "SubjectName", model.SubjectId);
            return View(model);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && item.EducationUnitId == this.Teacher.EducationUnitId)
            {
                var groups = await groupsInTestService.FindByAsync(git => git.TestId == item.Id);
                if (groups.Count() != 0)
                    return Json($"This test is currently in use: Id = {item.Id} - TestName = {item.TestName}", JsonRequestBehavior.AllowGet);
                await testService.DeleteAsync(item);
                return Json($"Successfully deleted item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SetStatus(bool status, int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && item.EducationUnitId == this.Teacher.EducationUnitId)
            {
                if (item.IsOpen == status)
                    return Json($"Item: #{item.Id} - \"{item.TestName}\" already has such status", JsonRequestBehavior.AllowGet);
                item.IsOpen = status;
                await testService.AddOrUpdateAsync(item);
                return Json($"Successfully set status \"{status}\" for item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
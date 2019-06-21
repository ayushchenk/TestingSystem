using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.Controllers
{
    //  [Authorize(Roles = "Admin")]
    public class GroupController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupInTestService;
        private IEntityService<TeachersInGroupDTO> teacherInGroupService;

        public GroupController(IEntityService<GroupDTO> groupService,
                               IEntityService<StudentDTO> studentService,
                               IEntityService<TeacherDTO> teacherService,
                               IEntityService<EducationUnitDTO> unitService,
                               IEntityService<SpecializationDTO> specService,
                               IEntityService<GroupsInTestDTO> groupInTestService,
                               IEntityService<TeachersInGroupDTO> teacherInGroupService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.groupInTestService = groupInTestService;
            this.teacherInGroupService = teacherInGroupService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await groupService.FindByAsync(group => group.GroupName.ToLower().Contains(filter.ToLower())
                                                                        || group.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                        || group.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await groupService.GetAllAsync());
        }

        public async Task<ActionResult> Students(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupDetailsViewModel
            {
                Students = await studentService.FindByAsync(user => user.GroupId == group.Id),
                Group = group
            };
            return View(model);
        }

        public async Task<ActionResult> Teachers(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var ids = teacherInGroupService.FindBy(tig => tig.GroupId == group.Id).Select(x => x.TeacherId);
            var model = new GroupDetailsViewModel
            {
                Teachers = await teacherService.FindByAsync(teacher => ids.Contains(teacher.Id)),
                Group = group
            };
            return View(model);
        }

        public async Task<ActionResult> Tests(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupTestsViewModel();
            model.Group = group;
            foreach (var item in await groupInTestService.FindByAsync(git => git.GroupId == group.Id))
                model.Tests.Add(item);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new CreateGroupViewModel();
            model.Group = new GroupDTO();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            return View("Edit", model);
        }


        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = new CreateGroupViewModel();
            model.Group = await groupService.GetAsync(id);
            if (model.Group == null)
                return RedirectToAction("Index");
            model.Teachers = teacherInGroupService.FindBy(tig => tig.GroupId == model.Group.Id).Select(tig => tig.TeacherId).ToList();
            model.SpecTeachers = await teacherService.FindByAsync(teacher => teacher.SpecializationId == model.Group.SpecializationId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.Group.EducationUnitId);
            //SelectList[] teachersList = new SelectList[model.Teachers.Count];
            //var ids = teacherInGroupService.FindBy(tig => tig.GroupId == model.Group.Id).Select(x => x.TeacherId);
            //var query = teacherService.FindBy(teacher => teacher.SpecializationId == model.Group.SpecializationId).Select(teacher => new { Id = teacher.Id, Teacher = $"{teacher.FirstName} {teacher.LastName} - {teacher.SubjectName}" });
            //for (int i = 0; i < model.Teachers.Count; i++)
            //    teachersList[i] = new SelectList(query, "Id", "Teacher", query.ElementAt(i).Id);
            //ViewBag.TeachersList = teachersList;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Group.Id != 0)
                    await groupService.DeleteAsync(model.Group);

                model.Group = await groupService.AddOrUpdateAsync(model.Group);

                foreach (var id in model.Teachers.Distinct())
                    teacherInGroupService.AddOrUpdate(new TeachersInGroupDTO() { TeacherId = id, GroupId = model.Group.Id });
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.Group.EducationUnitId);
            //SelectList[] teachersList = new SelectList[model.Teachers.Count];
            //var query = teacherService.GetAll().Select(teacher => new { Id = teacher.Id, Teacher = $"{teacher.FirstName} {teacher.LastName} - {teacher.SubjectName}" });
            //for (int i = 0; i < model.Teachers.Count; i++)
            //    teachersList[i] = new SelectList(query, "Id", "Teacher", model.Teachers[i]);
            //ViewBag.TeachersList = teachersList;
            return View(model);
        }

        public async Task<ActionResult> Cancel(int id = 0)
        {
            var item = await groupInTestService.GetAsync(id);
            if (item != null)
            {
                await groupInTestService.DeleteAsync(item);
                return RedirectToAction("Tests", item.GroupId);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await groupService.GetAsync(id);
            if (item != null)
            {
                var users = await studentService.FindByAsync(user => user.GroupId == item.Id);
                if (users.Count() != 0)
                    return Json($"There are users relying on such group: Id = {item.Id}, GroupName = {item.GroupName}", JsonRequestBehavior.AllowGet);
                await groupService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.GroupName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
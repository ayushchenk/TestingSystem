using AspNetIdentity.Managers;
using Microsoft.AspNet.Identity.Owin;
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
    [Authorize(Roles = "Education Unit Admin, Global Admin")]
    public class GroupController : Controller
    {
        private AdminDTO admin;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupInTestService;
        private IEntityService<TeachersInGroupDTO> teacherInGroupService;

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        public GroupController(IEntityService<GroupDTO> groupService,
                               IEntityService<AdminDTO> adminService,
                               IEntityService<StudentDTO> studentService,
                               IEntityService<TeacherDTO> teacherService,
                               IEntityService<EducationUnitDTO> unitService,
                               IEntityService<SpecializationDTO> specService,
                               IEntityService<GroupsInTestDTO> groupInTestService,
                               IEntityService<TeachersInGroupDTO> teacherInGroupService)
        {
            this.specService = specService;
            this.unitService = unitService;
            this.adminService = adminService;
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
            IEnumerable<GroupDTO> groups;
            if (this.Admin.IsGlobal)
                groups = await groupService.GetAllAsync();
            else
                groups = await groupService.FindByAsync(group => group.EducationUnitId == this.Admin.EducationUnitId);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(groups.Where(group => group.GroupName.ToLower().Contains(filter.ToLower())
                                                                        || group.SpecializationName.ToLower().Contains(filter.ToLower())
                                                                        || group.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(groups);
        }

        public async Task<ActionResult> Students(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupDetailsViewModel();
            model.Group = group;
            model.Students = await studentService.FindByAsync(student => student.GroupId == group.Id);
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
            if (this.Admin == null)
                return RedirectToAction("Index");
            var model = new CreateGroupViewModel();
            model.Group = new GroupDTO();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName");
            ViewBag.EducationUnits = new SelectList(this.Admin.IsGlobal ? await unitService.GetAllAsync() : await unitService.FindByAsync(unit => unit.Id == this.Admin.EducationUnitId), "Id", "EducationUnitName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Group.EducationUnitId = this.Admin.EducationUnitId.Value;
                model.Group = await groupService.AddOrUpdateAsync(model.Group);

                foreach (var id in model.Teachers.Distinct())
                    teacherInGroupService.AddOrUpdate(new TeachersInGroupDTO() { TeacherId = id, GroupId = model.Group.Id });
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(this.Admin.IsGlobal ? await unitService.GetAllAsync() : await unitService.FindByAsync(unit => unit.Id == this.Admin.EducationUnitId), "Id", "EducationUnitName");
            return View(model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = new CreateGroupViewModel();
            model.Group = await groupService.GetAsync(id);
            if (model.Group == null || this.Admin == null || (model.Group.EducationUnitId != (this.Admin.EducationUnitId ?? 0) && !this.Admin.IsGlobal))
                return RedirectToAction("Index");
            model.Teachers = teacherInGroupService.FindBy(tig => tig.GroupId == model.Group.Id).Select(tig => tig.TeacherId).ToList();
            model.SpecTeachers = await teacherService.FindByAsync(teacher => teacher.SpecializationId == model.Group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(this.Admin.IsGlobal ? await unitService.GetAllAsync() : await unitService.FindByAsync(unit => unit.Id == this.Admin.EducationUnitId), "Id", "EducationUnitName", model.Group.EducationUnitId);
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Group.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var id in model.Teachers.Distinct())
                    teacherInGroupService.AddOrUpdate(new TeachersInGroupDTO() { TeacherId = id, GroupId = model.Group.Id });
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.Group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(this.Admin.IsGlobal ? await unitService.GetAllAsync() : await unitService.FindByAsync(unit => unit.Id == this.Admin.EducationUnitId), "Id", "EducationUnitName", model.Group.EducationUnitId);
            return View(model);
        }

        public async Task<ActionResult> Cancel(int id = 0)
        {
            var item = await groupInTestService.GetAsync(id);
            if (item != null && this.Admin != null && (item.EducationUnitId == (this.Admin.EducationUnitId ?? 0) || this.Admin.IsGlobal))
            {
                await groupInTestService.DeleteAsync(item);
                return RedirectToAction("Tests", item.GroupId);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteTeacher(int teacher = 0, int group = 0)
        {
            var item = teacherInGroupService.FindBy(tig => tig.TeacherId == teacher && tig.GroupId == group).FirstOrDefault();
            if (item != null && this.Admin != null && (item.EducationUnitId == (this.Admin.EducationUnitId) || this.Admin.IsGlobal))
                await teacherInGroupService.DeleteAsync(item);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await groupService.GetAsync(id);
            if (item != null && this.Admin != null && (item.EducationUnitId == this.Admin.EducationUnitId || this.Admin.IsGlobal))
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
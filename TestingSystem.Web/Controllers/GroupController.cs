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
        private IEntityService<UserDTO> userService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupInTestService;

        public GroupController(IEntityService<UserDTO> userService, 
                               IEntityService<GroupDTO> groupService, 
                               IEntityService<EducationUnitDTO> unitService,
                               IEntityService<SpecializationDTO> specService, 
                               IEntityService<GroupsInTestDTO> groupInTestService)
        {
            this.userService = userService;
            this.specService = specService;
            this.unitService = unitService;
            this.groupService = groupService;
            this.groupInTestService = groupInTestService;
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

        public async Task<ActionResult> Users(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupDetailsViewModel
            {
                Users = await userService.FindByAsync(user => user.GroupId == group.Id),
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

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await groupService.GetAsync(id) ?? new GroupDTO();
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnitId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(GroupDTO group)
        {
            if (ModelState.IsValid)
            {
                await groupService.AddOrUpdateAsync(group);
                return RedirectToAction("Index");
            }
            ViewBag.Specializations = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", group.SpecializationId);
            ViewBag.EducationUnits = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", group.EducationUnitId);
            return View(group);
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
                var users = await userService.FindByAsync(user => user.GroupId == item.Id);
                if (users.Count() != 0)
                    return Json($"There are users relying on such group: Id = {item.Id}, GroupName = {item.GroupName}", JsonRequestBehavior.AllowGet);
                await groupService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.GroupName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
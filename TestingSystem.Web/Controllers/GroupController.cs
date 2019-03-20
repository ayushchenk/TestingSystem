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
    [Authorize(Roles = "Admin")]
    public class GroupController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<UserDTO> userService;

        public GroupController(IEntityService<GroupDTO> groupService, IEntityService<UserDTO> userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex()
        {
            return PartialView(await groupService.GetAllAsync());
        }

        public async Task<ActionResult> Users(int id = 0)
        {
            var group = await groupService.GetAsync(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupDetailsViewModel();
            model.Users = await userService.FindByAsync(user => user.GroupId == group.Id);
            model.Group = group;
            return View(model);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await groupService.GetAsync(id) ?? new GroupDTO();
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
            return View(group);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await groupService.GetAsync(id);
            if (item != null)
            {
                var users = await userService.FindByAsync(user => user.GroupId == item.Id);
                if (users.Count() != 0)
                    return Json($"There are users relying on such group: Id = {item.Id}, SpecName = {item.GroupName}", JsonRequestBehavior.AllowGet);
                await groupService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.GroupName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
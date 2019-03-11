using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult PartialIndex()
        {
            return PartialView(groupService.GetAll());
        }

        public ActionResult Details(int id = 0)
        {
            var group = groupService.Get(id);
            if (group == null)
                return RedirectToAction("Index");
            var model = new GroupDetailsViewModel();
            model.Group = group;
            model.Users = userService.FindBy(user => user.GroupId == group.Id);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = groupService.Get(id) ?? new GroupDTO();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(GroupDTO group)
        {
            if (ModelState.IsValid)
            {
                groupService.AddOrUpdate(group);
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult Delete(int id = 0)
        {
            var item = groupService.Get(id);
            if (item != null)
            {
                var users = userService.FindBy(user => user.GroupId == item.Id).ToList();
                if (users.Count() != 0)
                    return Json($"There are users relying os such group: Id = {item.Id}, SpecName = {item.GroupName}", JsonRequestBehavior.AllowGet);
                groupService.Delete(item);
                return Json($"Successfully deleted: {item.Id} - {item.GroupName}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
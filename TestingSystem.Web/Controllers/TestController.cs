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
    public class TestController : Controller
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;

        public TestController(IEntityService<TestDTO> testService, IEntityService<GroupDTO> groupService, IEntityService<GroupsInTestDTO> groupsInTestService)
        {
            this.testService = testService;
            this.groupService = groupService;
            this.groupsInTestService = groupsInTestService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex()
        {
            return PartialView(await testService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await testService.GetAsync(id) ?? new TestDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TestDTO model)
        {
            if (ModelState.IsValid)
            {
                await testService.AddOrUpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                var groups = await groupsInTestService.FindByAsync(git => git.TestId == item.Id && git.EndTime > DateTime.Now);
                if (groups.Count() != 0)
                    return Json($"This test is currently in use: Id = {item.Id} - TestName = {item.TestName}", JsonRequestBehavior.AllowGet);
                await testService.DeleteAsync(item);
                return Json($"Successfully deleted item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SetStatus(bool status, int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                if(item.IsOpen == status)
                    return Json($"Item: #{item.Id} - \"{item.TestName}\" already has such status", JsonRequestBehavior.AllowGet);
                item.IsOpen = status;
                await testService.AddOrUpdateAsync(item);
                return Json($"Successfully set status \"{status}\" for item: #{item.Id} - \"{item.TestName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item with such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AssignGroups(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                AssignGroupsViewModel model = new AssignGroupsViewModel();
                model.TestId = item.Id;
                foreach (var group in groupService.GetAll())
                    model.Groups.Add(new AssignGroupItem() { Group = group});
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AssignGroups(AssignGroupsViewModel model)
        {
            if(ModelState.IsValid)
            {
                model.Groups = model.Groups.Where(item => item.Assign == true).ToList();
                foreach (var item in model.Groups)
                    await groupsInTestService.AddOrUpdateAsync(new GroupsInTestDTO()
                    {
                        GroupId = item.Group.Id,
                        TestId = model.TestId
                    });
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
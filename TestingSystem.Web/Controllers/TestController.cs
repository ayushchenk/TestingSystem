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
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;

        public TestController(IEntityService<TestDTO> testService, IEntityService<GroupDTO> groupService, IEntityService<SpecializationDTO> specService, IEntityService<GroupsInTestDTO> groupsInTestService)
        {
            this.testService = testService;
            this.specService = specService;
            this.groupService = groupService;
            this.groupsInTestService = groupsInTestService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await testService.FindByAsync(test => test.SpecializationName.ToLower().Contains(filter.ToLower())));
            return PartialView(await testService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await testService.GetAsync(id) ?? new TestDTO();
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
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
            ViewBag.SpecializationId = new SelectList(await specService.GetAllAsync(), "Id", "SpecializationName", model.SpecializationId);
            return View(model);
        }

        public async Task<JsonResult> Delete(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                var groups = await groupsInTestService.FindByAsync(git => git.TestId == item.Id);
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
                if (item.IsOpen == status)
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
                var groupIds = groupsInTestService.GetAll().Select(git => git.GroupId);
                AssignGroupsViewModel model = new AssignGroupsViewModel();
                model.TestId = item.Id;
                foreach (var group in await groupService.FindByAsync(group => !groupIds.Contains(group.Id)))
                    model.Groups.Add(new AssignGroupItem() { Group = group });
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AssignGroups(AssignGroupsViewModel model)
        {
            //if(ModelState.IsValid)
            {
                model.Groups = model.Groups.Where(item => item.Assign == true).ToList();
                foreach (var item in model.Groups)
                    await groupsInTestService.AddOrUpdateAsync(new GroupsInTestDTO()
                    {
                        GroupId = item.Group.Id,
                        TestId = model.TestId,
                        StartDate = item.StartDate,
                        StartTime = item.StartTime,
                        Length = item.Length
                    });
                return RedirectToAction("Index");
            }
            //return View(model);
        }
    }
}
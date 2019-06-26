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
    public class GroupsInTestController : Controller
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;

        public GroupsInTestController(IEntityService<TestDTO> testService, 
                                      IEntityService<GroupDTO> groupService, 
                                      IEntityService<SpecializationDTO> specService, 
                                      IEntityService<GroupsInTestDTO> groupsInTestService)
        {
            this.testService = testService;
            this.specService = specService;
            this.groupService = groupService;
            this.groupsInTestService = groupsInTestService;
        }

        public async Task<ActionResult> AssignGroups(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null)
            {
                var alreadyAssigned = groupsInTestService.FindBy(git => git.TestId == id).Select(git => git.GroupId);
                var model = new AssignGroupsViewModel();
                model.TestId = id;
                foreach (var group in await groupService.FindByAsync(group => !alreadyAssigned.Contains(group.Id) && group.SpecializationId == item.SpecializationId))
                    model.Groups.Add(new AssignGroupItem
                    {
                        Group = group
                    });
                return View(model);
            }
            return RedirectToAction("Index", "Test");
        }

        [HttpPost]
        public async Task<ActionResult> AssignGroups(AssignGroupsViewModel model)
        {
            //if(ModelState.IsValid)
            {
                model.Groups = model.Groups.Where(item => item.Assign == true).ToList();
                foreach (var item in model.Groups)
                {
                    item.GroupInTest.GroupId = item.Group.Id;
                    item.GroupInTest.TestId = model.TestId;
                    await groupsInTestService.AddOrUpdateAsync(item.GroupInTest);
                }
                return RedirectToAction("Index", "Test");
            }
            //return View(model);
        }

        public async Task<ActionResult> Groups(int id = 0)
        {
            var groupsInTests = groupsInTestService.FindBy(git => git.TestId == id);
            var ids = groupsInTests.Select(git => git.GroupId);
            var groups = await groupService.FindByAsync(group => ids.Contains(group.Id));

            var model = new AssignGroupsViewModel();
            model.TestId = id;
            foreach (var git in groupsInTests)
                model.Groups.Add(new AssignGroupItem
                {
                    Group = groups.Where(group => group.Id == git.GroupId).First(),
                    GroupInTest = git
                });

            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await groupsInTestService.GetAsync(id);
            if (item != null)
                await groupsInTestService.DeleteAsync(item);
            return RedirectToAction("Groups", item.TestId);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var git = await groupsInTestService.GetAsync(id);
            var model = new AssignGroupItem
            {
                Group = await groupService.GetAsync(git.GroupId),
                GroupInTest = git
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AssignGroupItem model)
        {
            if (ModelState.IsValid)
            {
                await groupsInTestService.AddOrUpdateAsync(model.GroupInTest);
                return RedirectToAction("Index", "Test");
            }
            return View(model);
        }
    }
}
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
    [Authorize(Roles = "Teacher")]
    public class GroupsInTestController : Controller
    {
        private TeacherDTO teacher;
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<GroupsInTestDTO> groupsInTestService;
        private IEntityService<TeachersInGroupDTO> teacherInGroupsService;

        private TeacherDTO Teacher
        {
            get
            {
                if (teacher == null)
                    teacher = teacherService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return teacher;
            }
        }

        private IEnumerable<TeachersInGroupDTO> TeacherInGroups
        {
            get
            {
                return teacherInGroupsService.FindBy(tig => tig.TeacherId == this.Teacher.Id);
            }
        }

        private IEnumerable<int> GroupIds
        {
            get
            {
                return teacherInGroupsService.FindBy(tig => tig.TeacherId == this.Teacher.Id).Select(tig => tig.GroupId);
            }
        }

        public GroupsInTestController(IEntityService<TestDTO> testService,
                                      IEntityService<GroupDTO> groupService,
                                      IEntityService<TeacherDTO> teacherService,
                                      IEntityService<GroupsInTestDTO> groupsInTestService,
                                      IEntityService<TeachersInGroupDTO> teacherInGroupsService)
        {
            this.testService = testService;
            this.groupService = groupService;
            this.teacherService = teacherService;
            this.groupsInTestService = groupsInTestService;
            this.teacherInGroupsService = teacherInGroupsService;
        }

        public async Task<ActionResult> AssignGroups(int id = 0)
        {
            var item = await testService.GetAsync(id);
            if (item != null && !item.IsDeleted)
            {
                var filteredGroups = this.TeacherInGroups.Where(tig => tig.SubjectId == item.SubjectId).Select(gr => gr.GroupId);
                var model = new AssignGroupsViewModel();
                model.TestId = id;
                foreach (var group in await groupService.FindByAsync(group => filteredGroups.Contains(group.Id)))
                    model.Groups.Add(new AssignGroupItem { GroupInTest = new GroupsInTestDTO { GroupId = group.Id, GroupName = group.GroupName } });
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
            if (groupsInTests.Count() == 0)
                return View(model: null);
            var ids = groupsInTests.Select(git => git.GroupId);
            var groups = await groupService.FindByAsync(group => ids.Contains(group.Id) && this.GroupIds.Contains(group.Id));

            var model = new AssignGroupsViewModel();
            model.TestId = id;
            foreach (var git in groupsInTests.Where(g => g.StartTime > DateTime.Now))
                model.Groups.Add(new AssignGroupItem { GroupInTest = git });
            model.History = groupsInTests.Where(g => g.StartTime < DateTime.Now);

            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await groupsInTestService.GetAsync(id);
            if (item != null && this.GroupIds.Contains(item.GroupId))
                await groupsInTestService.DeleteAsync(item);
            return RedirectToAction("Groups", item.TestId);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var git = await groupsInTestService.GetAsync(id);
            if (!this.GroupIds.Contains(git.GroupId))
                return RedirectToAction("Index", "Test");
            var model = new AssignGroupItem { GroupInTest = git };
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
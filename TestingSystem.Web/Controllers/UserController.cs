using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using System.Threading.Tasks;
using TestingSystem.Web.Models.ViewModels;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using MailSender;
using System.Linq;
using System.Collections.Generic;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private IEntityService<UserDTO> userService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<SpecializationDTO> specService;
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        public UserController(IEntityService<UserDTO> userService, IEntityService<GroupDTO> groupService, IEntityService<SpecializationDTO> specService)
        {
            this.userService = userService;
            this.groupService = groupService;
            this.specService = specService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialIndex()
        {
            return PartialView(userService.GetAll());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = new EditUserViewModel();
            model.User = userService.Get(id) ?? new UserDTO();
            AppUser appUser = await UserManager.FindByEmailAsync(model.User.Email);
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name", UserManager.GetRoles(appUser.Id).First());
            ViewBag.Group = new SelectList(groupService.GetAll(), "Id", "GroupName", model.User.GroupId);
            ViewBag.Specialization = new SelectList(specService.GetAll(), "Id", "SpecializationName", model.User.SpecializationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO oldUser = userService.Get(model.User.Id);
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        await UserManager.RemoveFromRolesAsync(appUser.Id, UserManager.GetRoles(appUser.Id).ToArray());
                        UserManager.AddToRole(appUser.Id, model.Role);
                        appUser.Email = model.User.Email;
                        appUser.UserName = model.User.Login;
                        oldUser.Email = appUser.Email;
                        oldUser.Login = appUser.UserName;
                        oldUser.FirstName = model.User.FirstName;
                        oldUser.LastName = model.User.LastName;
                        oldUser.Patronymic = model.User.Patronymic;
                        oldUser.GroupId = model.Group;
                        oldUser.SpecializationId = model.Specialization;
                        await UserManager.UpdateAsync(appUser);
                        userService.AddOrUpdate(oldUser);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.Specialization = new SelectList(specService.GetAll(), "SpecializationName", "SpecializationName");
            ViewBag.Group = new SelectList(groupService.GetAll(), "GroupName", "GroupName");
            return View(model);
        }

        public ActionResult Create(int groupId = 0, int specId = 0)
        {
            var model = new EditUserViewModel();
            model.User = new UserDTO();
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.Group = new SelectList(groupService.GetAll(), "Id", "GroupName", groupId);
            ViewBag.Specialization = new SelectList(specService.GetAll(), "Id", "SpecializationName", specId);
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(EditUserViewModel model)
        {       
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.User.Email, Email = model.User.Email };
                string password = Membership.GeneratePassword(10, 4);
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                    model.User.GroupId = model.Group;
                    model.User.SpecializationId = model.Specialization;
                    userService.AddOrUpdate(model.User);
                    MailService sender = new MailService();
                    sender.SendComplexMessage(model.User.Email, "TestingSystem", password);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.Specialization = new SelectList(specService.GetAll(), "SpecializationName", "SpecializationName");
            ViewBag.Group = new SelectList(groupService.GetAll(), "GroupName", "GroupName");
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            UserDTO user = userService.Get(id);
            if (user != null)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var res = await UserManager.DeleteAsync(appUser);
                    if (res.Succeeded)
                    {
                        userService.Delete(user);
                        return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
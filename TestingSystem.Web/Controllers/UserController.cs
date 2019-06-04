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
   // [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private IEntityService<UserDTO> userService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<EducationUnitDTO> unitService;

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

        public UserController(IEntityService<UserDTO> userService, IEntityService<GroupDTO> groupService, IEntityService<SpecializationDTO> specService, IEntityService<EducationUnitDTO> unitService)
        {
            this.userService = userService;
            this.groupService = groupService;
            this.specService = specService;
            this.unitService = unitService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PartialIndex(string filter = null)
        {
            if(!string.IsNullOrWhiteSpace(filter))
                return PartialView(await userService.FindByAsync(user => user.Email.ToLower().Contains(filter.ToLower()) 
                                                                      || user.Login.ToLower().Contains(filter.ToLower())
                                                                      || user.LastName.ToLower().Contains(filter.ToLower())
                                                                      || user.FirstName.ToLower().Contains(filter.ToLower())
                                                                      || user.Patronymic.ToLower().Contains(filter.ToLower())
                                                                      || user.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await userService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = new EditUserViewModel();
            model.User = await userService.GetAsync(id) ?? new UserDTO();
            AppUser appUser = await UserManager.FindByEmailAsync(model.User.Email);
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name", UserManager.GetRoles(appUser.Id).First());
            ViewBag.EducationUnit = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.User.EducationUnitId);
            ViewBag.Group = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.User.EducationUnitId), "Id", "GroupName", model.User.GroupId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO oldUser = await userService.GetAsync(model.User.Id);
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
                        await UserManager.UpdateAsync(appUser);
                        await userService.AddOrUpdateAsync(oldUser);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.EducationUnit = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.User.EducationUnitId);
            ViewBag.Group = new SelectList(await groupService.FindByAsync(group => group.EducationUnitId == model.User.EducationUnitId), "Id", "GroupName", model.User.GroupId);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new EditUserViewModel();
            model.User = new UserDTO();
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.EducationUnit = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName");
            ViewBag.Group = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
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
                    await userService.AddOrUpdateAsync(model.User);
                    MailService sender = new MailService();
                    await sender.SendComplexMessageAsync(model.User.Email, "TestingSystem", password);
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
            ViewBag.EducationUnit = new SelectList(await unitService.GetAllAsync(), "Id", "EducationUnitName", model.EducationUnit);
            ViewBag.Group = new SelectList(await groupService.GetAllAsync(), "Id", "GroupName");
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            UserDTO user = await userService.GetAsync(id);
            if (user != null)
            {
                AppUser appUser = await UserManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var res = await UserManager.DeleteAsync(appUser);
                    if (res.Succeeded)
                    {
                        await userService.DeleteAsync(user);
                        return Json($"Successfully deleted: #{user.Id} - \"{user.Email}\"", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json($"Error occured on deleting identity: Id = {user.Id} - UserName = {user.Email}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsByUnit(int id = 0)
        {
            var unit = unitService.Get(id);
            if (unit != null)
                return Json(groupService.FindBy(group => group.EducationUnitId == unit.Id).Select(group => new { Id = group.Id, GroupName = group.GroupName}), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
    }
}
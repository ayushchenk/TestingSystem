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

namespace TestingSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private IEntityService<UserDTO> userSerivce;
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


        public UserController(IEntityService<UserDTO> userSerivce, IEntityService<GroupDTO> groupService, IEntityService<SpecializationDTO> specService)
        {
            this.userSerivce = userSerivce;
            this.groupService = groupService;
            this.specService = specService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialIndex()
        {
            return PartialView(userSerivce.GetAll());
        }

        public ActionResult Edit(int id = 0)
        {
            var model = new EditUserViewModel();
            model.User = userSerivce.Get(id) ?? new UserDTO();
            ViewBag.Role = new SelectList(RoleManager.Roles, "Name", "Name");
            ViewBag.Group = new SelectList(groupService.GetAll(), "Id", "GroupName");
            ViewBag.Specialization = new SelectList(specService.GetAll(), "Id", "SpecializationName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel model)
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
                    userSerivce.AddOrUpdate(model.User);
                    MailService sender = new MailService();
                    sender.SendComplexMessage(model.User.Email, "TestingSystem", password);
                    return RedirectToAction("Login", "Account");
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
    }
}
using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using System.Web.Security;
using System.Threading.Tasks;
using TestingSystem.Web.Models.ViewModels;
using MailSender;

namespace TestingSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private IEntityService<UserDTO> userSerivce;
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


        public UserController(IEntityService<UserDTO> userSerivce)
        {
            this.userSerivce = userSerivce;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialIndex()
        {
            return PartialView(userSerivce.GetAll());
        }

        
    }
}
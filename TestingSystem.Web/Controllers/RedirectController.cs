using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace TestingSystem.Web.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Index()
        {
            if (User.IsInRole("Student"))
                return RedirectToAction("Tests", "StudentContent");
            if (User.IsInRole("Teacher"))
                return RedirectToAction("Welcome", "TeacherContent");
            if (User.IsInRole("Education Unit Admin"))
                return RedirectToAction("Index", "Group");
            if (User.IsInRole("Global Admin"))
                return RedirectToAction("Index", "Role");
            return RedirectToAction("Logout", "Account");
        }
    }
}
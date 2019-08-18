using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingSystem.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult _500()
        {
            return View();
        }

        public ActionResult _404()
        {
            return View();
        }
    }
}
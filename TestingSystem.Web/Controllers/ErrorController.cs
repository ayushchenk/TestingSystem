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
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult _404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
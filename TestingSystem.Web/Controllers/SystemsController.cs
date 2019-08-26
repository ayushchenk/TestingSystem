using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingSystem.Web.Controllers
{
    public class SystemsController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string name = "")
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            if (result == null || result.View == null)
            {
                Response.StatusCode = 404;
            }
            return View(name);
        }
    }
}
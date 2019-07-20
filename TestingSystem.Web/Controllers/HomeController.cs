using System.Web.Mvc;

namespace TestingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About() => View();

        public ActionResult Contact() => View();
    }
}
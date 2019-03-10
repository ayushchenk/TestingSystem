using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpecializationController : Controller
    {
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<UserDTO> userService;

        public SpecializationController(IEntityService<SpecializationDTO> specService, IEntityService<UserDTO> userService)
        {
            this.specService = specService;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult PartialIndex()
        {
            return PartialView(specService.GetAll());
        }

        public ActionResult Edit(int id = 0)
        {
            var model = specService.Get(id) ?? new SpecializationDTO();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SpecializationDTO spec)
        {
            if (ModelState.IsValid)
            {
                specService.AddOrUpdate(spec);
                return RedirectToAction("Index");
            }
            return View(spec);
        }

        public ActionResult Delete(int id = 0)
        {
            var item = specService.Get(id);
            if (item != null)
            {
                var users = userService.FindBy(user => user.SpecializationId == item.Id);
                if (users.Count() != 0)
                    return Json($"There are users relying os such specialization: Id = {item.Id}, SpecName = {item.SpecializationName}", JsonRequestBehavior.AllowGet);
                specService.Delete(item);
                return Json($"Successfully deleted: {item.Id} - {item.SpecializationName}", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
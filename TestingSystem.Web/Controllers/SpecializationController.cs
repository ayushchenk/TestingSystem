using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

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

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await specService.FindByAsync(spec=> spec.SpecializationName.ToLower().Contains(filter.ToLower())));
            return PartialView(await specService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await specService.GetAsync(id) ?? new SpecializationDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SpecializationDTO spec)
        {
            if (ModelState.IsValid)
            {
                await specService.AddOrUpdateAsync(spec);
                return RedirectToAction("Index");
            }
            return View(spec);
        }

        public async Task<ActionResult> Users(int id = 0)
        {
            var spec = await specService.GetAsync(id);
            if (spec == null)
                return RedirectToAction("Index");
            var model = new SpecUsersViewModel();
            model.Users = await userService.FindByAsync(user => user.SpecializationId == spec.Id);
            model.Specialization = spec;
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await specService.GetAsync(id);
            if (item != null)
            {
                var users = await userService.FindByAsync(user => user.SpecializationId == item.Id);
                if (users.Count() != 0)
                    return Json($"There are users relying os such specialization: Id = {item.Id}, SpecName = {item.SpecializationName}", JsonRequestBehavior.AllowGet);
                await specService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.SpecializationName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
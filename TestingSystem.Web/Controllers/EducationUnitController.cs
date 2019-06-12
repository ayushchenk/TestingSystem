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
    //[Authorize(Roles = "Admin")]
    public class EducationUnitController : Controller
    {
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<GroupDTO> groupsService;

        public EducationUnitController(IEntityService<EducationUnitDTO> unitService,
                                       IEntityService<GroupDTO> groupsService)
        {
            this.unitService = unitService;
            this.groupsService = groupsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await unitService.FindByAsync(unit => unit.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(await unitService.GetAllAsync());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await unitService.GetAsync(id) ?? new EducationUnitDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EducationUnitDTO spec)
        {
            if (ModelState.IsValid)
            {
                await unitService.AddOrUpdateAsync(spec);
                return RedirectToAction("Index");
            }
            return View(spec);
        }

        public async Task<ActionResult> Groups(int id = 0)
        {
            var unit = await unitService.GetAsync(id);
            if (unit == null)
                return RedirectToAction("Index");
            var model = new EducationUnitGroupsViewModel
            {
                Groups = await groupsService.FindByAsync(group => group.SpecializationId == unit.Id),
                EducationUnit = unit
            };
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await unitService.GetAsync(id);
            if (item != null)
            {
                var groups = await groupsService.FindByAsync(group => group.SpecializationId == item.Id);
                if (groups.Count() != 0)
                    return Json($"There are groups relying os such unit: Id = {item.Id}, UnitName = {item.EducationUnitName}", JsonRequestBehavior.AllowGet);
                await unitService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.EducationUnitName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
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
    [Authorize(Roles = "Education Unit Admin, Global Admin")]
    public class EducationUnitController : Controller
    {
        private AdminDTO admin;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<GroupDTO> groupsService;
        private IEntityService<EducationUnitDTO> unitService;

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        public EducationUnitController(IEntityService<AdminDTO> adminService,
                                       IEntityService<GroupDTO> groupsService,
                                       IEntityService<EducationUnitDTO> unitService)
        {
            this.unitService = unitService;
            this.adminService = adminService;
            this.groupsService = groupsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            IEnumerable<EducationUnitDTO> units;
            if (this.Admin.IsGlobal)
                units = await unitService.GetAllAsync();
            else
                units = await unitService.FindByAsync(unit => unit.Id == this.admin.EducationUnitId);
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(units.Where(unit => unit.EducationUnitName.ToLower().Contains(filter.ToLower())));
            return PartialView(units);
        }

        [Authorize(Roles = "Global Admin")]
        public ActionResult Create()
        {
            return View("Edit", new EducationUnitDTO());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await unitService.GetAsync(id);
            if (model == null || this.Admin == null)
                return RedirectToAction("Index");
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
            if (item != null && (this.Admin.IsGlobal || item.Id == this.Admin.EducationUnitId))
            {
                var groups = await groupsService.FindByAsync(group => group.EducationUnitId == item.Id);
                if (groups.Count() != 0)
                    return Json($"There are groups relying os such unit: Id = {item.Id}, UnitName = {item.EducationUnitName}", JsonRequestBehavior.AllowGet);
                await unitService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.EducationUnitName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
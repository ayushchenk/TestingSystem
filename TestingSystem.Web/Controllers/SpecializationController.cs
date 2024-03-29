﻿using System;
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
    public class SpecializationController : Controller
    {
        private AdminDTO admin;
        private IEntityService<TestDTO> testService;
        private IEntityService<AdminDTO> adminService;
        private IEntityService<TeacherDTO> userService;
        private IEntityService<GroupDTO> groupsService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<SpecializationDTO> specService;

        private AdminDTO Admin
        {
            get
            {
                if (admin == null)
                    admin = adminService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return admin;
            }
        }

        public SpecializationController(IEntityService<TeacherDTO> userService,
                                        IEntityService<TestDTO> testService,
                                        IEntityService<AdminDTO> adminService,
                                        IEntityService<GroupDTO> groupsService,
                                        IEntityService<SubjectDTO> subjectService,
                                        IEntityService<SpecializationDTO> specService)
        {
            this.testService = testService;
            this.specService = specService;
            this.userService = userService;
            this.adminService = adminService;
            this.groupsService = groupsService;
            this.subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> PartialIndex(string filter = null)
        {
            ViewBag.IsGlobal = this.Admin?.IsGlobal ?? false;
            ViewBag.EducationUnitId = this.Admin?.EducationUnitId;
            if (!String.IsNullOrWhiteSpace(filter))
                return PartialView(await specService.FindByAsync(spec => spec.SpecializationName.ToLower().Contains(filter.ToLower())));
            return PartialView(await specService.GetAllAsync());
        }

        public ActionResult Create()
        {
            return View(viewName: "Edit", model: new SpecializationDTO());
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var model = await specService.GetAsync(id);
            if (this.Admin == null || model == null || ((model.EducationUnitId ?? 0) != (this.Admin.EducationUnitId ?? 0) && !this.Admin.IsGlobal))
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SpecializationDTO spec)
        {
            if (ModelState.IsValid)
            {
                spec.EducationUnitId = this.Admin.EducationUnitId;
                await specService.AddOrUpdateAsync(spec);
                return RedirectToAction("Index");
            }
            return View(spec);
        }

        public async Task<ActionResult> Groups(int id = 0)
        {
            var spec = await specService.GetAsync(id);
            if (spec == null || this.Admin == null)
                return RedirectToAction("Index");
            var model = new SpecGroupsViewModel
            {
                Groups = await groupsService.FindByAsync(group => group.SpecializationId == spec.Id),
                Specialization = spec
            };
            return View(model);
        }

        public async Task<ActionResult> Subjects(int id = 0)
        {
            var spec = await specService.GetAsync(id);
            if (spec == null || this.Admin == null)
                return RedirectToAction("Index");
            var model = new SpecSubjectsViewModel
            {
                Subjects = await subjectService.FindByAsync(subject => subject.SpecializationId == spec.Id),
                Specialization = spec
            };
            return View(model);
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            var item = await specService.GetAsync(id);
            if (item != null && this.Admin != null && ((item.EducationUnitId ?? 0) == (this.Admin.EducationUnitId ?? 0) || this.Admin.IsGlobal))
            {
                var tests = await testService.FindByAsync(test => test.SpecializationId == item.Id);
                var groups = await groupsService.FindByAsync(group => group.SpecializationId == item.Id);
                var subjects = await subjectService.FindByAsync(subject => subject.SpecializationId == item.Id);
                if (groups.Count() != 0)
                    return Json($"There are groups relying on such specialization: Id = {item.Id}, SpecName = {item.SpecializationName}", JsonRequestBehavior.AllowGet);
                if (tests.Count() != 0)
                    return Json($"There are tests relying on such specialization: Id = {item.Id}, SpecName = {item.SpecializationName}", JsonRequestBehavior.AllowGet);
                if (subjects.Count() != 0)
                    return Json($"There are subjects relying on such specialization: Id = {item.Id}, SpecName = {item.SpecializationName}", JsonRequestBehavior.AllowGet);
                await specService.DeleteAsync(item);
                return Json($"Successfully deleted: #{item.Id} - \"{item.SpecializationName}\"", JsonRequestBehavior.AllowGet);
            }
            return Json($"No item found by such id: {id}", JsonRequestBehavior.AllowGet);
        }
    }
}
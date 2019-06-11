using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    public class ServiceController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;

        public ServiceController(IEntityService<GroupDTO> groupService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService)
        {
            this.unitService = unitService;
            this.specService = specService;
            this.groupService = groupService;
            this.subjectService = subjectService;
        }

        public JsonResult GetSubjectsBySpecialization(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
                return Json(subjectService.FindBy(subject => subject.SpecializationId == spec.Id).Select(subject => new { Id = subject.Id, SubjectName = subject.SubjectName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsByUnit(int id = 0)
        {
            var unit = unitService.Get(id);
            if (unit != null)
                return Json(groupService.FindBy(group => group.EducationUnitId == unit.Id).Select(group => new { Id = group.Id, GroupName = group.GroupName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
    }
}
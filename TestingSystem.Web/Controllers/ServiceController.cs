using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.Controllers
{
    public class ServiceController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionAnswerDTO> answerService;

        public ServiceController(IEntityService<TeacherDTO> teacherService,
                                 IEntityService<GroupDTO> groupService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService,
                                 IEntityService<QuestionAnswerDTO> answerService)
        {
            this.teacherService = teacherService;
            this.unitService = unitService;
            this.specService = specService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.subjectService = subjectService;
        }

        public JsonResult GetSubjectsBySpecialization(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
                return Json(subjectService.FindBy(subject => subject.SpecializationId == spec.Id).Select(subject => new { Id = subject.Id, SubjectName = subject.SubjectName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeachersBySpecialization(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
                return Json(teacherService.FindBy(user => user.SpecializationId == spec.Id).Select(user => new { Id = user.Id, FullName = user.FullName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsByUnit(int id = 0)
        {
            var unit = unitService.Get(id);
            if (unit != null)
                return Json(groupService.FindBy(group => group.EducationUnitId == unit.Id).Select(group => new { Id = group.Id, GroupName = group.GroupName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task DeleteAnswer(int id = 0)
        {
            var item = await answerService.GetAsync(id);
            if (item != null)
                await answerService.DeleteAsync(item);
        }
    }
}
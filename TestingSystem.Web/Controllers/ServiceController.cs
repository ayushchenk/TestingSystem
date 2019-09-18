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
    public class ServiceController : Controller
    {
        private IEntityService<GroupDTO> groupService;
        private IEntityService<TeacherDTO> teacherService;
        private IEntityService<SubjectDTO> subjectService;
        private IEntityService<EducationUnitDTO> unitService;
        private IEntityService<SubjectThemeDTO> themeService;
        private IEntityService<SpecializationDTO> specService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<TeachersInSubjectDTO> teacherInSubjectService;

        public ServiceController(IEntityService<GroupDTO> groupService,
                                 IEntityService<TeacherDTO> teacherService,
                                 IEntityService<SubjectDTO> subjectService,
                                 IEntityService<SubjectThemeDTO> themeService,
                                 IEntityService<EducationUnitDTO> unitService,
                                 IEntityService<SpecializationDTO> specService,
                                 IEntityService<QuestionAnswerDTO> answerService,
                                 IEntityService<TeachersInSubjectDTO> teacherInSubjectService)
        {
            this.unitService = unitService;
            this.specService = specService;
            this.themeService = themeService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.teacherInSubjectService = teacherInSubjectService;
        }

        public JsonResult GetSubjectsBySpecialization(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
                return Json(subjectService.FindBy(subject => subject.SpecializationId == spec.Id).Select(subject => new { Id = subject.Id, SubjectName = subject.SubjectName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectsBySpecializationForTest(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
                return Json(subjectService.FindBy(subject => subject.SpecializationId == spec.Id).Where(subject => subject.Questions != 0).Select(subject => new { Id = subject.Id, SubjectName = subject.SubjectName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThemesBySubject(int id = 0)
        {
            var subject = subjectService.Get(id);
            if (subject != null)
                return Json(themeService.FindBy(theme => theme.SubjectId == subject.Id).Select(theme => new { Id = theme.Id, ThemeName = theme.ThemeName }), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThemesBySubjectForTest(int id = 0)
        {
            var subject = subjectService.Get(id);
            if (subject != null)
                return Json(themeService.FindBy(theme => theme.SubjectId == subject.Id && theme.Questions > 0).Select(theme => new { Id = theme.Id, ThemeName = theme.ThemeName, EasyCount = theme.EasyCount, MediumCount = theme.MediumCount, HardCount = theme.HardCount}), JsonRequestBehavior.AllowGet);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeachersBySpecialization(int id = 0)
        {
            var spec = specService.Get(id);
            if (spec != null)
            {
                var teachers = teacherService.FindBy(teacher => teacher.SpecializationId == spec.Id && !teacher.IsDeleted);
                if (teachers == null || teachers.Count() == 0)
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                var teacherSubjects = new List<TeacherSubject>();
                foreach (var teacher in teachers)
                {
                    var teachersInSubjects = teacherInSubjectService.FindBy(tis => tis.TeacherId == teacher.Id);
                    var subjectIds = teachersInSubjects.Select(tis => tis.SubjectId);
                    var subjects = subjectService.FindBy(subject => subjectIds.Contains(subject.Id) && subject.SpecializationId == spec.Id);
                    foreach (var subject in subjects)
                        teacherSubjects.Add(new TeacherSubject() { Teacher = teacher, Subject = subject, TeacherInSubjectId = teachersInSubjects.FirstOrDefault(t => t.TeacherId == teacher.Id && t.SubjectId == subject.Id)?.Id ?? 0 });
                }
                return Json(teacherSubjects.Select(user => new { Id = user.TeacherInSubjectId, FullName = user.Teacher.FirstName + " " + user.Teacher.LastName + " - " + user.Subject.SubjectName }), JsonRequestBehavior.AllowGet);
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.ApiControllers
{
    [Authorize]
    public class TeacherInSubjectsApiController : ApiController
    {
        private IEntityService<TeachersInSubjectDTO> teacherInSubjects;

        public TeacherInSubjectsApiController(IEntityService<TeachersInSubjectDTO> teacherInSubjects)
        {
            this.teacherInSubjects = teacherInSubjects;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] DeleteTeacherInSubject model)
        {
            if(model != null)
            {
                var item = await teacherInSubjects.FindByAsync(tis=> tis.TeacherId == model.TeacherId && tis.SubjectId == model.SubjectId);
                if (item.FirstOrDefault() != null)
                    await teacherInSubjects.DeleteAsync(item.FirstOrDefault());
                return item.FirstOrDefault().SubjectName;
            }
            return null;
        }
    }
}

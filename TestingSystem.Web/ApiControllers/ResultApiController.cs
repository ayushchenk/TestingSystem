using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;

namespace TestingSystem.Web.ApiControllers
{
    [Authorize]
    public class ResultApiController : ApiController
    {
        private IEntityService<StudentDTO> studentService;
        private IEntityService<StudentTestResultDTO> resultService;

        public ResultApiController(IEntityService<StudentDTO> studentService,
                                    IEntityService<StudentTestResultDTO> resultService)
        {
            this.resultService = resultService;
            this.studentService = studentService;
        }

        // GET: api/SubjectApi
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await resultService.GetAllAsync());
        }

        // GET: api/SubjectApi/5
        public async Task<IHttpActionResult> Get(string email)
        {
            email = HttpUtility.UrlDecode(email);
            var student = (await studentService.FindByAsync(st => st.Email == email)).FirstOrDefault();
            if (student != null)
                return Ok(await resultService.FindByAsync(result => result.StudentId == student.Id));
            else
                return BadRequest();
        }

        // POST: api/SubjectApi
        public async Task Post([FromBody]StudentTestResultDTO model)
        {
            await resultService.AddOrUpdateAsync(model);
        }
    }
}

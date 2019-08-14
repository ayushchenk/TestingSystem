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
    public class SubjectApiController : ApiController
    {
        private IEntityService<SubjectDTO> subjectService;

        public SubjectApiController(IEntityService<SubjectDTO> subjectService)
        {
            this.subjectService = subjectService;
        }

        // GET: api/SubjectApi
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await subjectService.FindByAsync(s => s.Questions > 0));
        }

        // GET: api/SubjectApi/5
        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok(await subjectService.GetAsync(id));
        }
    }
}

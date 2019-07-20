using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult Get()
        {
            return Ok(subjectService.GetAll());
        }

        // GET: api/SubjectApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SubjectApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SubjectApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SubjectApi/5
        public void Delete(int id)
        {
        }
    }
}

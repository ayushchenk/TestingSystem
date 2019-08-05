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
    public class TestApiController : ApiController
    {
        private IEntityService<GroupsInTestDTO> gitService;

        public TestApiController(IEntityService<GroupsInTestDTO> gitService)
        {
            this.gitService = gitService;
        }

        // GET: api/SubjectApi/5
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await gitService.GetAllAsync());
        }
    }
}

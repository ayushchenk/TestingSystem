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
    public class SpecializationApiController : ApiController
    {
        private IEntityService<SpecializationDTO> specializationService;

        public SpecializationApiController(IEntityService<SpecializationDTO> specializationService)
        {
            this.specializationService = specializationService;
        }

        // GET: api/SubjectApi
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await specializationService.GetAllAsync());
        }

        // GET: api/SubjectApi/5
        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok(await specializationService.GetAsync(id));
        }
    }
}

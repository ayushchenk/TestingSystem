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
    public class StudyingMaterialsApiController : ApiController
    {
        private IEntityService<StudyingMaterialDTO> materialService;

        public StudyingMaterialsApiController(IEntityService<StudyingMaterialDTO> materialService)
        {
            this.materialService = materialService;
        }

        // GET: api/SubjectApi
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await materialService.GetAllAsync());
        }
    }
}

using AspNetIdentity.Managers;
using Microsoft.AspNet.Identity.Owin;
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
using TestingSystem.Web.Models.Identity;

namespace TestingSystem.Web.ApiControllers
{
    public class LoginApiController : ApiController
    {
        private IEntityService<StudentDTO> studentService;

        private AppUserManager UserManager
        {
            get
            {
                var manager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                return manager;
            }
        }

        public LoginApiController(IEntityService<StudentDTO> studentService)
        {
            this.studentService = studentService;
        }

        // POST: api/SubjectApi
        public async Task<IHttpActionResult> Post([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest("All fields required");
            var user = await UserManager.FindAsync(model.Email, model.Password);
            if(user == null)
                return BadRequest("Wrong credentials");
            var student = (await studentService.FindByAsync(st=> st.Email == user.Email)).FirstOrDefault();
            if (student == null)
                return BadRequest("You are not student");
            return Ok(student.Id);
        }
    }
}
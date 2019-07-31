﻿using System;
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
    public class StudentApiController : ApiController
    {
        private IEntityService<StudentDTO> studentService;

        public StudentApiController(IEntityService<StudentDTO> studentService)
        {
            this.studentService = studentService;
        }

        // GET: api/SubjectApi
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await studentService.GetAllAsync());
        }

        // GET: api/SubjectApi/5
        public async Task<IHttpActionResult> Get(string email)
        {
            email = HttpUtility.UrlDecode(email);
            return Ok((await studentService.FindByAsync(student => student.Email == email)).FirstOrDefault());
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

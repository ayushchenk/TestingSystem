using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.XamarinForms.ApiServices
{
    public class SubjectService
    {
        const string Url = "https://localhost:44381/api/subjectapi";
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public IEnumerable<SubjectDTO> GetAll()
        {
            HttpClient client = GetClient();
            string result = client.GetStringAsync(Url).Result;
            return JsonConvert.DeserializeObject<IEnumerable<SubjectDTO>>(result);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.ApiControllers
{
    public class QuickTestApiController : ApiController
    {
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionAnswerDTO> answerService;

        public QuickTestApiController(IEntityService<QuestionDTO> questionService, IEntityService<QuestionAnswerDTO> answerService)
        {
            this.answerService = answerService;
            this.questionService = questionService;
        }

        public async Task<IHttpActionResult> Post([FromBody] QuickTestApiModel apiModel)
        {
            var questions = await questionService.FindByAsync(q => q.SubjectId == apiModel.SubjectId);
            var questionIds = questions.Select(q => q.Id);
            var answers = await answerService.FindByAsync(a => questionIds.Contains(a.QuestionId));
            ParticipateViewModel model = new ParticipateViewModel()
            {
                QuestionCount = apiModel.QuestionCount,
                Length = (int)(apiModel.QuestionCount * 1.5),
            };
            Random rnd = new Random();
            int realCount = Math.Min(model.QuestionCount, questions.Count());
            for (int i = 0; i < model.QuestionCount; i++)
            {
                int selId = rnd.Next(realCount);
                model.QuestionAnswers.Add(new QuestionAnswer
                {
                    Question = questions.ElementAt(selId),
                    Answers = answers.Where(ans => ans.QuestionId == questions.ElementAt(selId).Id).ToList()
                });
            }
            return Ok(model);
        }
    }

    public class QuickTestApiModel
    {
        public int SubjectId { set; get; }
        public int QuestionCount { set; get; }
    }
}

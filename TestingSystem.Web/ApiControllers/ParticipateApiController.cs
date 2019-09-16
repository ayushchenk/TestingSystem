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
    [Authorize]
    public class ParticipateApiController : ApiController
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupsInTestDTO> gitService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionAnswerDTO> answerService;

        public ParticipateApiController(IEntityService<TestDTO> testService,
                                        IEntityService<GroupsInTestDTO> gitService,
                                        IEntityService<QuestionDTO> questionService,
                                        IEntityService<QuestionAnswerDTO> answerService)
        {
            this.gitService = gitService;
            this.testService = testService;
            this.answerService = answerService;
            this.questionService = questionService;
        }

        // GET: api/ParticipateApi/6
        public async Task<IHttpActionResult> Get(int id)
        {
            var git = await gitService.GetAsync(id);
            if (git == null)
                return BadRequest("Git");
            var test = await testService.GetAsync(git.TestId);
            if (test == null)
                return BadRequest("Test");

            var questions = await questionService.FindByAsync(q => q.SubjectId == test.SubjectId);
            var questionIds = questions.Select(q => q.Id);
            var answers = await answerService.FindByAsync(a => questionIds.Contains(a.QuestionId));

            var easyQuestions = questions.Where(q => q.Difficulty == 1);
            var hardQuestions = questions.Where(q => q.Difficulty == 2);
            var mediumQuestions = questions.Where(q => q.Difficulty == 3);

            ParticipateViewModel model = new ParticipateViewModel()
            {
                StartTime = git.StartTime.Value,
                EndTime = git.StartTime.Value.AddMinutes(git.Length),
                GroupInTestId = git.Id,
                Length = git.Length,
                QuestionCount = test.EasyCount + test.MediumCount + test.HardCount,
                SubjectId = test.SubjectId
            };
            Random rnd = new Random();

            int realCount = Math.Min(test.EasyCount, easyQuestions.Count());
            for (int i = 0; i < test.EasyCount; i++)
            {
                int selId = rnd.Next(realCount);
                QuestionAnswer qa = new QuestionAnswer();
                qa.Question = easyQuestions.ElementAt(selId);
                qa.Answers = answers.Where(ans => ans.QuestionId == qa.Question.Id).ToList();
                model.QuestionAnswers.Add(qa);
            }

            realCount = Math.Min(test.MediumCount, mediumQuestions.Count());
            for (int i = 0; i < test.EasyCount; i++)
            {
                int selId = rnd.Next(realCount);
                QuestionAnswer qa = new QuestionAnswer();
                qa.Question = mediumQuestions.ElementAt(selId);
                qa.Answers = answers.Where(ans => ans.QuestionId == qa.Question.Id).ToList();
                model.QuestionAnswers.Add(qa);
            }

            realCount = Math.Min(test.HardCount, hardQuestions.Count());
            for (int i = 0; i < test.EasyCount; i++)
            {
                int selId = rnd.Next(realCount);
                QuestionAnswer qa = new QuestionAnswer();
                qa.Question = hardQuestions.ElementAt(selId);
                qa.Answers = answers.Where(ans => ans.QuestionId == qa.Question.Id).ToList();
                model.QuestionAnswers.Add(qa);
            }
            return Ok(model);
        }
    }
}

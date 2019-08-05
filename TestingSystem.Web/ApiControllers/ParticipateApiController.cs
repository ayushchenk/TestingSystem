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
    public class ParticipateApiController : ApiController
    {
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<GroupsInTestDTO> gitService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<StudentTestResultDTO> resultService;
        private IEntityService<StudyingMaterialDTO> materialService;

        public ParticipateApiController(IEntityService<TestDTO> testService,
                                        IEntityService<GroupDTO> groupService,
                                        IEntityService<StudentDTO> studentService,
                                        IEntityService<GroupsInTestDTO> gitService,
                                        IEntityService<QuestionDTO> questionService,
                                        IEntityService<QuestionAnswerDTO> answerService,
                                        IEntityService<StudentTestResultDTO> resultService,
                                        IEntityService<StudyingMaterialDTO> materialService)
        {
            this.gitService = gitService;
            this.testService = testService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.resultService = resultService;
            this.studentService = studentService;
            this.materialService = materialService;
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
            ParticipateViewModel model = new ParticipateViewModel()
            {
                StartTime = git.StartTime.Value,
                EndTime = git.StartTime.Value.AddMinutes(git.Length),
                GroupInTestId = git.Id,
                Length = git.Length,
                QuestionCount = test.QuestionCount,
                SubjectId = test.SubjectId
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
}

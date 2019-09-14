using AspNetIdentity.Managers;
using AspNetIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.Web.Models.ViewModels;

namespace TestingSystem.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentContentController : Controller
    {
        private StudentDTO student;
        private IEntityService<TestDTO> testService;
        private IEntityService<GroupDTO> groupService;
        private IEntityService<StudentDTO> studentService;
        private IEntityService<GroupsInTestDTO> gitService;
        private IEntityService<QuestionDTO> questionService;
        private IEntityService<QuestionAnswerDTO> answerService;
        private IEntityService<StudentTestResultDTO> resultService;
        private IEntityService<StudyingMaterialDTO> materialService;

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private int GroupId
        {
            get
            {
                return this.Student == null ? 0 : this.Student.GroupId;
            }
        }

        private StudentDTO Student
        {
            get
            {
                if (student == null)
                    student = studentService.FindBy(s => s.Email == User.Identity.Name).FirstOrDefault();
                return student;
            }
        }

        public StudentContentController(IEntityService<TestDTO> testService,
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

        public async Task<ActionResult> Tests()
        {
            var results = resultService.GetAll().Select(result => result.GroupInTestId);
            var model = await gitService.FindByAsync(git => git.GroupId == this.GroupId && !results.Contains(git.Id));
            model = model.Where(item => DateTime.Now <= item.StartTime.Value.AddMinutes(item.Length));
            return View(model);
        }

        public async Task<ActionResult> Participate(int id = 0)
        {
            if (Session["ParticipateModel"] == null)
            {
                if (this.Student == null)
                    return RedirectToAction("Tests");
                var history = (await resultService.FindByAsync(result => result.GroupId == this.Student.GroupId)).Select(result => result.GroupInTestId);
                var groupInTests = await gitService.FindByAsync(g => g.GroupId == this.Student.GroupId && !history.Contains(g.Id));
                var git = groupInTests.FirstOrDefault(g => g.Id == id);
                if (git == null)
                    return RedirectToAction("Tests");
                var test = await testService.GetAsync(git.TestId);
                if (test == null)
                    return RedirectToAction("Tests");
                var questions = await questionService.FindByAsync(q => q.SubjectId == test.SubjectId);
                var questionIds = questions.Select(q => q.Id);
                var answers = await answerService.FindByAsync(a => questionIds.Contains(a.QuestionId));
                ParticipateViewModel model = new ParticipateViewModel()
                {
                    StartTime = git.StartTime.Value,
                    EndTime = git.StartTime.Value.AddMinutes(git.Length),
                    GroupInTestId = git.Id,
                    StudentId = this.Student.Id,
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
                Session.Add("ParticipateModel", model);
                return View(model);
            }
            else
            {
                return View(Session["ParticipateModel"] as ParticipateViewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Participate(ParticipateViewModel model)
        {
            StudentTestResultDTO result = new StudentTestResultDTO
            {
                GroupInTestId = model.GroupInTestId,
                StudentId = model.StudentId
            };
            for (int i = 0; i < model.QuestionCount; i++)
            {
                switch (model.QuestionAnswers[i].QuestionType)
                {
                    case QuestionType.OneAnswerOneCorrect:
                        if (!String.IsNullOrWhiteSpace(model.QuestionAnswers[i].Answers[0].AnswerString)
                            && (model.QuestionAnswers[i].AnswerString?.Trim().ToLower() ?? String.Empty) == model.QuestionAnswers[i].Answers[0].AnswerString.Trim().ToLower())
                            result.Result++;
                        break;
                    case QuestionType.ManyAnswersOneCorrect:
                        if (model.QuestionAnswers[i].Answers.Find(answer => answer.IsCorrect).Id == model.QuestionAnswers[i].AnswerId)
                            result.Result++;
                        break;
                    case QuestionType.ManyAnswersManyCorrect:
                        {
                            var rightAnswers = model.QuestionAnswers[i].Answers.Where(answer => answer.IsCorrect).Select(answer => answer.Id);
                            var checkedAnswers = model.QuestionAnswers[i].Answers.Where(ch => ch.PickedCheckbox.Checked).Select(ch => ch.PickedCheckbox.AnswerId);
                            if (rightAnswers.SequenceEqual(checkedAnswers))
                                result.Result++;
                        }
                        break;
                }
            }
            model.Result = result.Result;
            Session.Remove("ParticipateModel");
            await resultService.AddOrUpdateAsync(result);
            return View("ParticipateResult", model);
        }

        public async Task<ActionResult> History()
        {
            if (this.Student == null)
                return RedirectToAction("Test");
            var model = await resultService.FindByAsync(result => result.StudentId == this.Student.Id);
            return View(model);
        }

        public async Task<ActionResult> Group()
        {
            if (this.GroupId == 0)
                return RedirectToAction("Test");
            var model = await studentService.FindByAsync(student => student.GroupId == this.GroupId);
            return View(model);
        }

        public async Task<ActionResult> StudyingMaterials()
        {
            if (this.Student == null)
                return RedirectToAction("Test");
            var model = await materialService.FindByAsync(material => material.SpecializationId == this.Student.SpecializationId);
            return View(model);
        }

        public new ActionResult Profile()
        {
            if (this.Student == null)
                return RedirectToAction("Test");
            return View(this.Student);
        }

        public ActionResult Edit()
        {
            if (this.Student == null)
                return RedirectToAction("Test");
            return View(this.Student);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StudentDTO model)
        {
            if (ModelState.IsValid)
            {
                StudentDTO oldUser = this.Student;
                if (oldUser != null)
                {
                    AppUser appUser = await UserManager.FindByEmailAsync(oldUser.Email);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Email;
                        await UserManager.UpdateAsync(appUser);
                        await studentService.AddOrUpdateAsync(model);
                        return RedirectToAction("Profile");
                    }
                }
            }
            return View(model);
        }
    }
}
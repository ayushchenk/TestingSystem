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
                if(student == null)
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
                                        IEntityService<StudentTestResultDTO> resultService)
        {
            this.gitService = gitService;
            this.testService = testService;
            this.groupService = groupService;
            this.answerService = answerService;
            this.studentService = studentService;
            this.resultService = resultService;
            this.questionService = questionService;
        }

        public async Task<ActionResult> Tests()
        {
            var results = resultService.GetAll().Select(result => result.GroupInTestId);
            var model = await gitService.FindByAsync(git => git.GroupId == this.GroupId && !results.Contains(git.Id));
            model = model.Where(item => item.StartTime.Value < DateTime.Now
                && item.StartTime.Value.AddMinutes(item.Length) > DateTime.Now);
            return View(model);
        }

        public async Task<ActionResult> Participate(int id = 0)
        {
            ParticipateViewModel model = null;
            if (Session["ParticipateModel"] == null)
            {
                var git = await gitService.GetAsync(id);
                if (git == null)
                    return RedirectToAction("Tests");
                var test = await testService.GetAsync(git.TestId);
                if (test == null)
                    return RedirectToAction("Tests");
                if (this.Student == null)
                    return RedirectToAction("Tests");
                var questions = await questionService.FindByAsync(q => q.SubjectId == test.SubjectId);
                var questionIds = questions.Select(q => q.Id);
                var answers = await answerService.FindByAsync(a => questionIds.Contains(a.QuestionId));
                model = new ParticipateViewModel(git.Length, test.SubjectId, test.QuestionCount);
                model.GroupInTestId = git.Id;
                model.StudentId = this.Student.Id;
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
                Session.Add("BeginTime", DateTime.Now);
            }
            else
            {
                model = Session["ParticipateModel"] as ParticipateViewModel;
            }
            if (model != null)
                return View(model);
            else
                return RedirectToAction("Test");
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
                        if (!String.IsNullOrWhiteSpace(model.PickedAnswers[i].AnswerString)
                            && model.QuestionAnswers[i].Answers[0].AnswerString.Trim().ToLower() == model.PickedAnswers[i].AnswerString.Trim().ToLower())
                            result.Result++;
                        break;
                    case QuestionType.ManyAsnwersOneCorrect:
                        if (model.PickedAnswers[i].AnswerId != 0
                            && model.QuestionAnswers[i].Answers.Find(answer => answer.IsCorrect).Id == model.PickedAnswers[i].AnswerId)
                            result.Result++;
                        break;
                    case QuestionType.ManyAnswersManyCorrect:
                        if (model.PickedAnswers[i].PickedCheckboxes != null
                           && model.PickedAnswers[i].PickedCheckboxes.Count != 0)
                        {
                            var rightAnswers = model.QuestionAnswers[i].Answers.Where(answer => answer.IsCorrect).Select(answer => answer.Id);
                            var checkedAnswers = model.PickedAnswers[i].PickedCheckboxes.Where(ch => ch.Checked).Select(ch => ch.AnswerId);
                            if (rightAnswers.SequenceEqual(checkedAnswers))
                                result.Result++;
                        }
                        break;
                }
            }
            Session["ParticipateModel"] = null;
            await resultService.AddOrUpdateAsync(result);
            return RedirectToAction("Test");
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
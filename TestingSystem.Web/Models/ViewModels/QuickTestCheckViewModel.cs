using System.Collections.Generic;

namespace TestingSystem.Web.Models.ViewModels
{
    public class QuickTestCheckViewModel
    {
        public int SpecializationId { set; get; }
        public int SubjectId { set; get; }
        public int QuestionCount { set; get; }
        public List<QuestionAnswer> QuestionAnswers { set; get; } = new List<QuestionAnswer>();
    }
}
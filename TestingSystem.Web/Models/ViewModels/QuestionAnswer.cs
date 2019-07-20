using System.Collections.Generic;
using System.Linq;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class QuestionAnswer
    {
        public QuestionDTO Question { set; get; }
        public List<QuestionAnswerDTO> Answers { set; get; } = new List<QuestionAnswerDTO>();

        public string AnswerString { set; get; }
        public int AnswerId { set; get; }

        public QuestionType QuestionType
        {
            get
            {
                int allCount = Answers.Count();
                int correctCount = Answers.Count(answer => answer.IsCorrect);
                if (allCount > 1 && correctCount > 1) return QuestionType.ManyAnswersManyCorrect;
                if (allCount > 1 && correctCount == 1) return QuestionType.ManyAsnwersOneCorrect;
                if (allCount == 1 && correctCount == 1) return QuestionType.OneAnswerOneCorrect;
                return QuestionType.Undefined;
            }
        }
    }

    public enum QuestionType
    {
        ManyAsnwersOneCorrect,
        ManyAnswersManyCorrect,
        OneAnswerOneCorrect,
        Undefined
    }
}
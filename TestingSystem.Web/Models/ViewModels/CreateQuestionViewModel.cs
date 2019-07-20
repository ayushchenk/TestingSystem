using System.Collections.Generic;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateQuestionViewModel
    {
        public QuestionDTO Question { set; get; } = new QuestionDTO();
        public List<QuestionAnswerDTO> Answers { set; get; } = new List<QuestionAnswerDTO>(10);
    }
}
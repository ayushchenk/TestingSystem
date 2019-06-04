using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateQuestionViewModel
    {
        public QuestionDTO Question { set; get; } = new QuestionDTO();
        public List<QuestionAnswerDTO> Answers { set; get; } = new List<QuestionAnswerDTO>(10);
    }
}
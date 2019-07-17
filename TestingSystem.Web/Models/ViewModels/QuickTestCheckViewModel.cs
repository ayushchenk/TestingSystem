using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class QuickTestCheckViewModel
    {
        public int SubjectId { set; get; }
        public int Length { set; get; }
        public int QuestionCount { set; get; }
        public List<QuestionAnswer> QuestionAnswers { set; get; } = new List<QuestionAnswer>();
        public List<PickedAnswer> PickedAnswers { set; get; } = new List<PickedAnswer>();
    }
}
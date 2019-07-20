using System;
using System.Collections.Generic;

namespace TestingSystem.Web.Models.ViewModels
{
    public class ParticipateViewModel
    {
        public int GroupInTestId { set; get; }
        public int StudentId { set; get; }
        public int Length { set; get; }
        public int SubjectId { set; get; }
        public DateTime EndTime { set; get; }
        public DateTime StartTime { set; get; }
        public int QuestionCount { set; get; }
        public int Result { set; get; }
        public List<QuestionAnswer> QuestionAnswers { set; get; } = new List<QuestionAnswer>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public List<QuestionAnswer> QuestionAnswers { set; get; }
        public List<PickedAnswer> PickedAnswers { set; get; }

        public ParticipateViewModel() { }

        public ParticipateViewModel(int length, int subjectId, int questionCount)
        {
            this.Length = length;
            this.SubjectId = subjectId;
            this.QuestionCount = questionCount;
            this.QuestionAnswers = new List<QuestionAnswer>();
            this.PickedAnswers = new List<PickedAnswer>();
            PickedAnswers = new List<PickedAnswer>(QuestionCount);
            for (int i = 0; i < QuestionCount; i++)
                PickedAnswers.Add(new PickedAnswer());
        }
    }
}
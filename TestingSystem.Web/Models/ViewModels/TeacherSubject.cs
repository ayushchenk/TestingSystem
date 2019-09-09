using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class TeacherSubject
    {
        public TeacherDTO Teacher { set; get; }
        public SubjectDTO Subject { set; get; }
        public int TeacherInSubjectId { set; get; }
    }
}
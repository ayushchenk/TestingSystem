using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestingSystem.Web.Models.ViewModels
{
    public class DeleteTeacherInSubject
    {
        public int TeacherId { set; get; }
        public int SubjectId { set; get; }
    }
}
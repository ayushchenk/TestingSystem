using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateTeacherViewModel
    {
        public TeacherDTO Teacher { set; get; }
        public IList<SelectListItem> Subjects { set; get; } = new List<SelectListItem>();
        public IEnumerable<int> SelectedSubjects { set; get; } = new List<int>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateTestViewModel
    {
        public TestDTO Test { set; get; }
        public IList<SubjectThemeDTO> Themes { set; get; }
        public IList<SelectListItem> SelectThemeItems { set; get; }
        public IEnumerable<int> SelectedThemes { set; get; } = new List<int>();
    }
}
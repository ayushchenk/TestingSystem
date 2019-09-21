using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateGroupViewModel
    {
        public GroupDTO Group { set; get; }
        public IList<TeachersInSubjectDTO> Teachers { set; get; } = new List<TeachersInSubjectDTO>();
        public IList<SelectListItem> SelectTeacherItems { set; get; } = new List<SelectListItem>();
        public IEnumerable<int> SelectedTeachers  { set; get; } = new List<int>();
    }
}
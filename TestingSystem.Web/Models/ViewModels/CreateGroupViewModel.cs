using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class CreateGroupViewModel
    {
        public GroupDTO Group { set; get; }
        public List<int> Teachers { set; get; } = new List<int>();
        public IEnumerable<TeacherDTO> SpecTeachers { set; get; } = new List<TeacherDTO>();
    }
}
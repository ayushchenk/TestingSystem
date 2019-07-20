using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class GroupDetailsViewModel
    {
        public GroupDTO Group { set; get; }
        public IEnumerable<StudentDTO> Students { set; get; }
        public IEnumerable<TeacherDTO> Teachers{ set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class TeacherGroupSubjects
    {
        public GroupDTO Group { get; set; }
        public IEnumerable<TeachersInGroupDTO> Subjects { set; get; }
    }
}
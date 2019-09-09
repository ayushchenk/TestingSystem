using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AddTeacherViewModel
    {
        public GroupDTO Group { set; get; }
        public IEnumerable<TeachersInSubjectDTO> Teachers { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int TeacherId { set; get; }
    }
}
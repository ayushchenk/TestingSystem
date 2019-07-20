using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AddTeacherViewModel
    {
        public GroupDTO Group { set; get; }
        public IEnumerable<TeacherDTO> Teachers { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select")]
        public int TeacherId { set; get; }
    }
}
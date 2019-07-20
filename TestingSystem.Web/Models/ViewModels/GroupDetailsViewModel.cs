using System.Collections.Generic;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class GroupDetailsViewModel
    {
        public GroupDTO Group { set; get; }
        public IEnumerable<StudentDTO> Students { set; get; }
        public IEnumerable<TeacherDTO> Teachers { set; get; }
    }
}
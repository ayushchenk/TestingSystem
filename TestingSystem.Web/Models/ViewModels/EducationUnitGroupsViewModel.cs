using System.Collections.Generic;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EducationUnitGroupsViewModel
    {
        public EducationUnitDTO EducationUnit { set; get; }
        public IEnumerable<GroupDTO> Groups { set; get; }
    }
}
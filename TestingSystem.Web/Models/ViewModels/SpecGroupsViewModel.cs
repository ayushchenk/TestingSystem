using System.Collections.Generic;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class SpecGroupsViewModel
    {
        public SpecializationDTO Specialization { set; get; }
        public IEnumerable<GroupDTO> Groups { set; get; }
    }
}
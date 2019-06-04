using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EducationUnitGroupsViewModel
    {
        public EducationUnitDTO EducationUnit { set; get; }
        public IEnumerable<GroupDTO> Groups { set; get; }
    }
}
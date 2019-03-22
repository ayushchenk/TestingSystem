using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class GroupTestsViewModel
    {
        public GroupTestsViewModel()
        {
            Tests = new List<GroupsInTestDTO>();
        }

        public GroupDTO Group { set; get; }
        public IList<GroupsInTestDTO> Tests { set; get; }
    }
}
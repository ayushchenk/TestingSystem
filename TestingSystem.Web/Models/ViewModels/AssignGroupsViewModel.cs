using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AssignGroupsViewModel
    {
        public AssignGroupsViewModel()
        {
            Groups = new List<AssignGroupItem>();
        }

        public IEnumerable<GroupsInTestDTO> History { set; get; }

        [Required]
        public IList<AssignGroupItem> Groups { set; get; }

        [Required]
        public int TestId { set; get; }
    }
}
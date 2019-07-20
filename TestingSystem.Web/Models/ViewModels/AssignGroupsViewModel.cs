using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AssignGroupsViewModel
    {
        public AssignGroupsViewModel()
        {
            Groups = new List<AssignGroupItem>();
        }

        [Required]
        public IList<AssignGroupItem> Groups { set; get; }
        [Required]
        public int TestId { set; get; }
    }
}
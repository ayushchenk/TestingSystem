using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AssignGroupItem
    {
        public AssignGroupItem()
        {
            Group = new GroupDTO();
            GroupInTest = new GroupsInTestDTO();
        }

        [Required]
        public GroupDTO Group { set; get; }
        public GroupsInTestDTO GroupInTest { set; get; }
        public bool Assign { set; get; }
    }
}
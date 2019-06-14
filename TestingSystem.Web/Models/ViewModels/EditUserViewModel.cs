using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        public UserDTO User { set; get; }

        public string Role { set; get; }

        //public int Group { set; get; }

        //public int EducationUnit { set; get; }
    }
}
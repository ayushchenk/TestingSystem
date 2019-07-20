using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class IndexUserViewModel
    {
        public TeacherDTO User { set; get; }
        public string Role { set; get; }
    }
}
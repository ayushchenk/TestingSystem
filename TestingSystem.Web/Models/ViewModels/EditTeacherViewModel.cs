using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EditTeacherViewModel
    {
        public TeacherDTO Teacher { set; get; }
        public bool IsTwoFactorEnabled { set; get; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        //[Required]
        //[StringLength(64)]
        //public string Login { get; set; }

        //[Required]
        //[StringLength(64)]
        //public string Email { get; set; }

        //[StringLength(64)]
        //public string FirstName { get; set; }

        //[StringLength(64)]
        //public string LastName { get; set; }

        //[StringLength(64)]
        //public string Patronymic { get; set; }
        public UserDTO User { set; get; }

        public string Role { set; get; }

        public int Group { set; get; }

        public int Specialization { set; get; }
    }
}
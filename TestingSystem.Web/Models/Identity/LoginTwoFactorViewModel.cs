using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestingSystem.Web.Models.Identity
{
    public class LoginTwoFactorViewModel
    {
        public LoginViewModel Login { set; get; }
        public int VerificationCode { set; get; }

        [Range(100000, 999999, ErrorMessage = "Code must be 6-digit")]
        [Compare("VerificationCode", ErrorMessage = "Code dismatch")]
        public int InputCode { set; get; }
    }
}
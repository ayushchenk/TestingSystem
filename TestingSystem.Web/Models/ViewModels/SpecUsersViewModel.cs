using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class SpecUsersViewModel
    {
        public SpecializationDTO Specialization { set; get; }
        public IEnumerable<UserDTO> Users { set; get; }
    }
}
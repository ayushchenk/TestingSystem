using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class SpecSubjectsViewModel
    {
        public SpecializationDTO Specialization { set; get; }
        public IEnumerable<SubjectDTO> Subjects { set; get; }
    }
}
using System.Collections.Generic;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class SpecSubjectsViewModel
    {
        public SpecializationDTO Specialization { set; get; }
        public IEnumerable<SubjectDTO> Subjects { set; get; }
    }
}
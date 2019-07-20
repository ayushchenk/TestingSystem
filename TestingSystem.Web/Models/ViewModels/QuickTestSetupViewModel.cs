using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class QuickTestSetupViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Plese select")]
        public int SpecializationId { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Plese select")]
        public int SubjectId { set; get; }

        [Range(1, 100, ErrorMessage = "From 1 to 100")]
        public int QuestionCount { set; get; }

        public IEnumerable<SpecializationDTO> Specializations { set; get; }
        public IEnumerable<SubjectDTO> Subjects { set; get; }
    }
}
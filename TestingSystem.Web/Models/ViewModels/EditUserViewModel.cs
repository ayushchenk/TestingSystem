using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        public TeacherDTO User { set; get; }

        public string Role { set; get; }
    }
}
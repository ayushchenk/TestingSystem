using TestingSystem.BusinessModel.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AssignGroupItem
    {
        public AssignGroupItem()
        {
            Group = new GroupDTO();
            GroupInTest = new GroupsInTestDTO();
        }

        public GroupDTO Group { set; get; }
        public GroupsInTestDTO GroupInTest { set; get; }
        public bool Assign { set; get; }
    }
}
using GenericRepository.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingSystem.DAL.DbModel
{
    public partial class GroupsInTest:Entity<int>
    {
        [NotMapped]
        int Entity<int>.Id
        {
            get { return this.Id; }
            set { this.Id = value; }
        }
    }
}

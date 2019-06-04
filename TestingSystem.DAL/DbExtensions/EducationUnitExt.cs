using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.DAL.DbModel
{
    public partial class EducationUnit:Entity<int>
    {
        [NotMapped]
        int Entity<int>.Id
        {
            get { return this.Id; }
            set { this.Id = value; }
        }
    }
}

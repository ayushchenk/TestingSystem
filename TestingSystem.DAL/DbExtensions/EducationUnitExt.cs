using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.DAL.DbModel
{
    public partial class EducationUnit:IEntity<int>
    {
        [NotMapped]
        int IEntity<int>.Id
        {
            get { return this.Id; }
            set { this.Id = value; }
        }
    }
}

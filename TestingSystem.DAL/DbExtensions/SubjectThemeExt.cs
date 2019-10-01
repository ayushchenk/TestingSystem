using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.DAL.DbModel
{
    public partial class SubjectTheme:IEntity<int>
    {
        [NotMapped]
        int IEntity<int>.Id
        {
            set { this.Id = value; }
            get { return this.Id; }
        }
    }
}

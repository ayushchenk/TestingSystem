using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class GroupsInTestDTO
    {
        public int Id { get; set; }

        public int Length { set; get; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [StringLength(32)]
        public string GroupName { get; set; }

        [StringLength(128)]
        public string TestName { get; set; }

        public int GroupId { get; set; }

        public int TestId { get; set; }
    }
}

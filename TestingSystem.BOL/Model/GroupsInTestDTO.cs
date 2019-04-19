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

        [Column(TypeName = "time")]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        public int Length { set; get; }

        public int GroupId { get; set; }

        public int TestId { get; set; }

        [MaxLength(10)]
        public string GroupName { get; set; }

        [MaxLength(20)]
        public string TestName { get; set; }
    }
}

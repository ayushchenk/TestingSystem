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

        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        public int GroupId { get; set; }

        public int TestId { get; set; }

        [Required]
        [StringLength(64)]
        public string GroupName { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }
    }
}

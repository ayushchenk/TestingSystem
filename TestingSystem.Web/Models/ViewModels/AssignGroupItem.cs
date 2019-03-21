using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestingSystem.BOL.Model;

namespace TestingSystem.Web.Models.ViewModels
{
    public class AssignGroupItem
    {
        public AssignGroupItem()
        {
            Group = new GroupDTO();
        }

        [Required]
        public GroupDTO Group { set; get; }
        public bool Assign { set; get; }
        public int Length { set; get; }
        public TimeSpan StartTime { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { set; get; }
    }
}
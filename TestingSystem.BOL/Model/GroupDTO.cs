﻿using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string GroupName { get; set; }
    }
}

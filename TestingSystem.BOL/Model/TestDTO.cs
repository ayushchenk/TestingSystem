using System;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class TestDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string TestName { get; set; }

        [Required]
        public bool IsOpen { set; get; }

        [Required]
        public int SpecializationId { set; get; }

        [MaxLength(40)]
        public string SpecializationName { get; set; }
    }
}

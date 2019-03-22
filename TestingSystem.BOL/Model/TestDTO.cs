using System;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class TestDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }

        [Required]
        public bool IsOpen { set; get; }

        [Required]
        public int SpecializationId { set; get; }

        [StringLength(128)]
        public string SpecializationName { get; set; }
    }
}

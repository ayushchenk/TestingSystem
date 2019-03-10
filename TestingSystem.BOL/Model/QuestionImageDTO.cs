using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionImageDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string ImagePath { get; set; }

        [StringLength(32)]
        public string MimeType { get; set; }
    }
}

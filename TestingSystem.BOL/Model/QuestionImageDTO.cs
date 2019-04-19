using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionImageDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ImagePath { get; set; }

        [MaxLength(15)]
        public string MimeType { get; set; }
    }
}

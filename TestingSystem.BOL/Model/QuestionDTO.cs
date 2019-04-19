using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string QuestionString { get; set; }

        public int? ImageId { get; set; }

        public int SpecializationId { set; get; }

        [MaxLength(40)]
        public string SpecializationName { get; set; }

        [MaxLength(200)]
        public string ImagePath { get; set; }

        [MaxLength(15)]
        public string MimeType { get; set; }
    }
}

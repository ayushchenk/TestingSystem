using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionString { get; set; }

        public int? ImageId { get; set; }

        public int TestId { get; set; }

        [Required]
        [StringLength(128)]
        public string TestName { get; set; }

        [Required]
        [StringLength(256)]
        public string ImagePath { get; set; }

        [StringLength(32)]
        public string MimeType { get; set; }
    }
}

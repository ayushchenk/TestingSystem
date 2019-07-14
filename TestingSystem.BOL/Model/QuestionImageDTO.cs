using System.ComponentModel.DataAnnotations;

namespace TestingSystem.BOL.Model
{
    public class QuestionImageDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string ImagePath { get; set; }

        public int? EducationUnitId { set; get; }

        public string FileName
        {
            get
            {
                var parts = ImagePath.Split('/');
                return parts[parts.Length - 1];
            }
        }
    }
}

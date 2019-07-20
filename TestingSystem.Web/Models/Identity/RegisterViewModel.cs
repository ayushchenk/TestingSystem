using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.Web.Models.Identity
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { set; get; }

        [Required]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
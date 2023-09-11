using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem_Assign2.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [CustomValidationForPassword(ErrorMessage = "Password must be at least 6 characters long and contain at least 1 uppercase letter and 1 special character.")]
        public string Password { get; set; }
    }
}

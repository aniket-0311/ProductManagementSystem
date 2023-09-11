using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem_Assign2.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

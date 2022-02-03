using System.ComponentModel.DataAnnotations;

namespace Helperland.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

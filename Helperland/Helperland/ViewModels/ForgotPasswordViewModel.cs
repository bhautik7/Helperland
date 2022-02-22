using System.ComponentModel.DataAnnotations;

namespace Helperland.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter an email")]
        public string Email { get; set; }
    }
}

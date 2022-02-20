using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Please enter First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter E-mail address")]
        [EmailAddress(ErrorMessage = "Please enter Valid email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Mobile number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter valid Phone Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,16}$", ErrorMessage = "You must enter At least one upper case, one lower case, one digit and Minimum six and Maximum 16 in length")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}

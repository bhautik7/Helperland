using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Helperland.ViewModels
{
    public class ContactUsViewModel
    {
#nullable disable

        [Required(ErrorMessage = "Please Enter Your Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email Address")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Enter Valid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone]
        [StringLength(10)]
        public string MobileNumber { get; set; }
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter Your Message")]
        public string Message { get; set; }

#nullable enable
        public IFormFile? File { get; set; }
    }
}

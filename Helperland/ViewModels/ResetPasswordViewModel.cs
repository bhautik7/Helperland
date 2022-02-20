using Microsoft.AspNetCore.Http;

namespace Helperland.ViewModels
{
    public class ResetPasswordViewModel
    {
#nullable disable
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile Attachment { get; set; }
        public bool IsHTML { get; set; }
    }
}

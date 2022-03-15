using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class UserViewModel
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("userTypeId")]
        public int UserTypeId { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("userProfilePicture")]
        public string UserProfilePicture { get; set; }

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; }

        [JsonPropertyName("worksWithPets")]
        public bool WorksWithPets { get; set; }

        [JsonPropertyName("languageId")]
        public int LanguageId { get; set; }

        [JsonPropertyName("nationalityId")]
        public int NationalityId { get; set; }

        [JsonPropertyName("userAddress")]
        public UserAddressViewModel UserAddress { get; set; }
    }
}

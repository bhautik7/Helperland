using System.Text.Json.Serialization;

namespace Helperland.ViewModels
{
    public class UserAddressViewModel
    {
        [JsonPropertyName("streetName")]
        public string StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public string HouseNumber { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}

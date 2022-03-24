using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class EditServiceRequestAdminViewModel
    {
        [JsonPropertyName("serviceRequestId")]
        public int ServiceRequestId { get; set; }

        [JsonPropertyName("serviceStartDate")]
        public string ServiceStartDate { get; set; }

        [JsonPropertyName("serviceStartTime")]
        public string ServiceStartTime { get; set; }

        [JsonPropertyName("streetName")]
        public string StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public string HouseNumber { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

    }
}


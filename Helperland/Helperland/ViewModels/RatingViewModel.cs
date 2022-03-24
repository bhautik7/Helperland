using Helperland.Models;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class RatingViewModel
    {
        [JsonPropertyName("ratingId")]
        public int RatingId { get; set; }

        [JsonPropertyName("serviceRequestId")]
        public int ServiceRequestId { get; set; }

        [JsonPropertyName("ratingFrom")]
        public int RatingFrom { get; set; }

        [JsonPropertyName("ratingTo")]
        public int RatingTo { get; set; }

        [JsonPropertyName("ratings")]
        public decimal Ratings { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("ratingDate")]
        public DateTime RatingDate { get; set; }

        [JsonPropertyName("onTimeArrival")]
        public decimal OnTimeArrival { get; set; }

        [JsonPropertyName("friendly")]
        public decimal Friendly { get; set; }

        [JsonPropertyName("qualityOfService")]
        public decimal QualityOfService { get; set; }

        [JsonPropertyName("serviceProvider")]
        public User ServiceProvider { get; set; }

        [JsonPropertyName("serviceProviderRating")]
        public decimal ServiceProviderRating { get; set; }
    }
}

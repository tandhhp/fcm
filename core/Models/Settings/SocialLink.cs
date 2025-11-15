using System.Text.Json.Serialization;
using Waffle.Entities;

namespace Waffle.Models.Settings
{
    public class SocialLink : BaseEntity
    {
        [JsonPropertyName("facebook")]
        public string? Facebook { get; set; }
        [JsonPropertyName("instagram")]
        public string? Instagram { get; set; }
        [JsonPropertyName("twitter")]
        public string? Twitter { get; set; }
        [JsonPropertyName("youtube")]
        public string? Youtube { get; set; }
    }
}

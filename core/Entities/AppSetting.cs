using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Waffle.Entities
{
    public class AppSetting : BaseEntity
    {
        [StringLength(100), Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [StringLength(100), Required]
        [JsonPropertyName("normalizedName")]
        public string NormalizedName { get; set; } = string.Empty;
        [StringLength(500)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Waffle.Entities;

public class Localization : BaseEntity
{
    public Localization()
    {
        Language = "vi-VN";
        Key = string.Empty;
    }
    [Required, StringLength(10)]
    [JsonPropertyName("language")]
    public string Language { get; set; }
    [Required, StringLength(100)]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

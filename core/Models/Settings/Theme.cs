using System.Text.Json.Serialization;

namespace Waffle.Models.Settings;

public class Theme
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "default";
}

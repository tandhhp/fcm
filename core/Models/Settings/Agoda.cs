using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Settings;

public class Agoda : BaseSetting
{
    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }
    [JsonPropertyName("cId")]
    public string? CId { get; set; }
}

using System.Text.Json.Serialization;
using Waffle.Core.Interfaces;

namespace Waffle.Entities;

public class Component : BaseEntity, IComponent
{
    public Component()
    {
        Name = string.Empty;
        NormalizedName = string.Empty;
    }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("normalizedName")]
    public string NormalizedName { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

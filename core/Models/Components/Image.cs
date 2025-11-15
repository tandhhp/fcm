using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Image", Prompt = "image")]
public class Image : AbstractComponent
{
    [JsonPropertyName("alt")]
    public string Alt { get; set; } = "IMG";
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("link")]
    public Link Link { get; set; } = new();
    [JsonPropertyName("wrapper")]
    public string Wrapper { get; set; } = "wf-image";
    [JsonPropertyName("src")]
    public string? Src { get; set; }

    [JsonIgnore]
    public bool HasImage => !string.IsNullOrEmpty(Src);

    [JsonIgnore]
    public bool HasLink => !string.IsNullOrEmpty(Link.Name);
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Google Map", Prompt = "google-map")]
public class GoogleMap : AbstractComponent
{
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("src")]
    public string? Src { get; set; }
}

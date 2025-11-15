using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Sponsor", Prompt = "sponsor")]
public class Sponsor : AbstractComponent
{
    [JsonPropertyName("brands")]
    public List<SponsorBrand> Brands { get; set; } = new();
}

public class SponsorBrand
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = new Guid();
    [JsonPropertyName("logo")]
    public string? Logo { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

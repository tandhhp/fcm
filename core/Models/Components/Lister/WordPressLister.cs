using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.ExternalAPI.Models;

namespace Waffle.Models.Components;

[Display(Name = "WordPress Lister", Prompt = "wordpress-lister")]
public class WordPressLister : AbstractComponent
{
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = default!;
    [JsonPropertyName("apiVersion")]
    public int ApiVersion { get; set; }

    [JsonIgnore]
    [JsonPropertyName("category")]
    public string Category { get; set; } = default!;

    [JsonIgnore]
    public string? SearchTerm { get; set; }
    [JsonIgnore]
    public IEnumerable<WordPressPost> Posts { get; set; } = new List<WordPressPost>();
}

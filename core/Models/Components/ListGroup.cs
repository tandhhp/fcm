using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "List Group", Prompt = "list-group")]
public class ListGroup : AbstractComponent
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonIgnore]
    public bool HasBadge { get; set; }
    [JsonPropertyName("items")]
    public IEnumerable<ListGroupItem> Items { get; set; } = new List<ListGroupItem>();
}

public class ListGroupItem
{
    [JsonPropertyName("link")]
    public Link Link { get; set; } = new();
    [JsonPropertyName("icon")]
    public string Icon { get; set; } = "fab fa-staylinked";
    [JsonPropertyName("badge")]
    public int Badge { get; set; }
    [JsonPropertyName("suffix")]
    public string Suffix { get; set; } = string.Empty;
    [JsonIgnore]
    public bool HasSuffix => !string.IsNullOrEmpty(Suffix);
}

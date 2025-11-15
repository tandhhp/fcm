using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Models.Components;

[Display(Name = "Article Picker", Prompt = "article-picker")]
public class ArticlePicker : AbstractComponent
{
    [JsonPropertyName("tagId")]
    public Guid TagId { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Catalog> Articles { get; set; } = new();
}

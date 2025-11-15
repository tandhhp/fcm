using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Models.Components;

[Display(Name = "Article Spotlight", AutoGenerateField = true, Prompt = "article-spotlight")]
public class ArticleSpotlight : AbstractComponent
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 5;

    [JsonIgnore]
    public IEnumerable<Catalog> Articles { get; set; } = new List<Catalog>();
}

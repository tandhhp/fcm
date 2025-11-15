using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.ExternalAPI.Googles.Models;

namespace Waffle.Models.Components;

[Display(Name = "Blogger lister", Prompt = "blogger lister")]
public class BloggerLister : AbstractComponent
{
    [JsonPropertyName("blogId")]
    public string? BlogId { get; set; }
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;

    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    [JsonIgnore]
    public BloggerListResult<BloggerItem>? Posts { get; set; }
}

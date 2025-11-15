using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Models.Components;

[Display(Name = "Shopee Product", Prompt = "shopee-product")]
public class ShopeeProduct : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("urlSuffix")]
    public string? UrlSuffix { get; set; }
    [JsonPropertyName("groupId")]
    public string? GroupId { get; set; }

    [JsonIgnore]
    public IEnumerable<Catalog>? Products { get; set; }
}

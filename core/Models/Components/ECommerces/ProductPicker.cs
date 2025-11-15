using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Models.Components;

[Display(Name = "Product Picker", Prompt = "product-picker", GroupName = GroupName.ECommerce)]
public class ProductPicker : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("tagIds")]
    public List<Guid> TagIds { get; set; } = new();

    [JsonIgnore]
    public IEnumerable<Catalog>? Products { get; set; }
}

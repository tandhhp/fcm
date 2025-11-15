using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Product Images", Prompt = "product-images", GroupName = GroupName.ECommerce, AutoGenerateField = true)]
public class ProductImage : AbstractComponent
{
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = new();
}

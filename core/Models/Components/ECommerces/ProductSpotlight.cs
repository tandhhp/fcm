using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Models.Components;

[Display(Name = "Product spotlight", Prompt = "product-spotlight", AutoGenerateField = true)]
public class ProductSpotlight : AbstractComponent
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "Chúng tôi bán";
    [JsonPropertyName("itemPerRow")]
    public string ItemPerRow { get; set; } = "col-6";
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    [JsonIgnore]
    public IEnumerable<ProductListItem> Products { get; set; } = new List<ProductListItem>();
}

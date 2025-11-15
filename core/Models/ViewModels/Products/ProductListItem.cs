using Waffle.Entities;

namespace Waffle.Models.ViewModels.Products;

public class ProductListItem : Catalog
{
    public string Url { get; set; } = default!;
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int Point { get; set; }
    public string? SKU { get; set; }
    public string? Galleries { get; set; }
    public string? Summary { get; set; }

    public decimal Discount => 100 - Math.Round((SalePrice / Price ?? 1) * 100);
}

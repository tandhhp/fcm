using Waffle.Entities;

namespace Waffle.Models.ViewModels;

public class SpotlightListItem : BaseEntity
{
    public string Url { get; set; } = default!;
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public string? Thumbnail { get; set; }
    public string? Name { get; set; }
    public int ViewCount { get; set; }
    public DateTime ModifiedDate { get; set; }
}

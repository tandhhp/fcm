using Waffle.Entities;

namespace Waffle.Models.Filters;

public class TourFilterOptions
{
    public string? Location { get; set; }
    public string? Name { get; set; }
    public CatalogType Type { get; set; } = CatalogType.Tour;
    public Guid? ParentId { get; set; }
}

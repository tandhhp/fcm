using Waffle.Entities;
using Waffle.Entities.Tours;

namespace Waffle.Models.ViewModels;

public class TourViewModel : BaseEntity
{
    public Guid? CatalogId { get; set; }
    public Guid? ParentId { get; set; }
    public string? Location { get; set; }
    public List<Amenity> Amenities { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public int Point { get; set; }
    public string? Thumbnail { get; set; }
    public List<Itinerary> Itineraries { get; set; } = new();
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int ViewCount { get; set; }
    public CatalogType Type { get; set; }
}

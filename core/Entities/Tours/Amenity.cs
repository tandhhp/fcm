namespace Waffle.Entities.Tours;

public class Amenity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Icon { get; set; }
}

public class TourAmenity
{
    public Guid CatalogId { get; set; }
    public Guid AmenityId { get; set; }
}
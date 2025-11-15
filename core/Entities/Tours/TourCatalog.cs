namespace Waffle.Entities.Tours
{
    public class TourCatalog : BaseEntity
    {
        public string? Location { get; set; }
        public int Point { get; set; }
        public string? Images { get; set; }
        public string? Tags { get; set; }
        public string? Amenities { get; set; }
        public string? Content { get; set; }
        public Guid? CatalogId { get; set; }

        public Catalog? Catalog { get; set; }
    }
}

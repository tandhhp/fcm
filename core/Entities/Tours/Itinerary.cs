namespace Waffle.Entities.Tours
{
    public class Itinerary : BaseEntity
    {
        public Guid CatalogId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public Catalog? Catalog { get; set; }
    }
}

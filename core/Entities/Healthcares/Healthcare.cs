namespace Waffle.Entities.Healthcares;

public class Healthcare : BaseEntity
{
    public Guid? CatalogId { get; set; }
    public string? Location { get; set; }
    public int Point { get; set; }
    public int Time { get; set; }
    public string? Content { get; set; }

    public Catalog? Catalog { get; set; }
}

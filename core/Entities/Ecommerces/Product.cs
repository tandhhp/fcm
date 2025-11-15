namespace Waffle.Entities.Ecommerces;

public class Product : BaseEntity
{
    public Guid CatalogId { get; set; }
    public int Point { get; set; }
    public string? Content { get; set; }
    public string? Brand { get; set; }
    public string? SKU { get; set; }
    public int? UnitInStock { get; set; }
    public string? Galleries { get; set; }
    public string? Summary { get; set; }
    public string? Cert1File { get; set; }
    public string? Cert1Name{ get; set; }
    public string? Cert2File { get; set; }
    public string? Cert2Name { get; set; }
}

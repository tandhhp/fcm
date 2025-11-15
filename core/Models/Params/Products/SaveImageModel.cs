namespace Waffle.Models.Params.Products;

public class SaveImageModel
{
    public List<string> Urls { get; set; } = new();
    public Guid CatalogId { get; set; }
}

namespace Waffle.Models.Params;

public class AddComment
{
    public string Message { get; set; } = default!;
    public Guid CatalogId { get; set; }
    public Guid? ParrentId { get; set; }
    public int Rate { get; set; }
}

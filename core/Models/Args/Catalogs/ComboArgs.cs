namespace Waffle.Models.Args.Catalogs;

public class ComboArgs
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Content { get; set; }
    public string? Thumbnail { get; set; }
    public string? Description { get; set; }
    public Guid ParentId { get; set; }
    public string? Location { get; set; }
    public int Point { get; set; }
}

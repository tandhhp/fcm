namespace Waffle.Models.Params;

public class AddStyleModel
{
    public Guid CatalogId { get; set; }
    public Guid WorkContentId { get; set; }
    public bool Active { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AddWorkContentModel
{
    public string Name { get; set; } = string.Empty;
    public Guid CatalogId { get; set; }
    public Guid ComponentId { get; set; }
}

public class DeleteWorkContent
{
    public Guid WorkContentId { get; set; }
    public Guid CatalogId { get; set; }
}

public class DeleteNavItemModel
{
    public Guid LinkId { get; set; }
    public Guid WorkId { get; set; }
}

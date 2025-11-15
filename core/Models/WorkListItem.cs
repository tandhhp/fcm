using Waffle.Entities;

namespace Waffle.Models;

public class WorkListItem : BaseEntity
{
    public WorkListItem()
    {
        NormalizedName = string.Empty;
        Arguments = string.Empty;
    }
    public string? Name { get; set; }
    public string NormalizedName { get; set; }
    public string Arguments { get; set; }
    public int SortOrder { get; set; }
    public Guid CatalogId { get; set; }
    public bool Active { get; set; }
    public bool AutoGenerateField { get; set; }
}

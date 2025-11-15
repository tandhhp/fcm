using Waffle.Entities;
using Waffle.Entities.Tours;

namespace Waffle.Models.Args;

public class CxCreateArgs
{
    public CatalogType Type { get; set; }
    public FormStatus Status { get; set; }
    public Guid CardHolderId { get; set; }
    public Guid CatalogId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public string? Name { get; set; }
    public int? Point { get; set; }
}

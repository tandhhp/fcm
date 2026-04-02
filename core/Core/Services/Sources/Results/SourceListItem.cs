using Waffle.Entities;
using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Sources.Results;

public class SourceListItem : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public string? TypeOfData { get; set; }
    public SourceType? SourceType { get; set; }
    public bool Overwrite { get; set; }
    public bool Protected { get; set; }
    public int? TypeOfDataId { get; set; }
    public int ContactCount { get; set; }
    public string? TeamName { get; set; }
    public int? TeamId { get; set; }
}

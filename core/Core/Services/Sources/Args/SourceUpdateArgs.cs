using Waffle.Entities;

namespace Waffle.Core.Services.Sources.Args;

public class SourceUpdateArgs : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public int TeamId { get; set; }
    public int TypeOfDataId { get; set; }
    public bool Protected { get; set; }
    public bool Overwrite { get; set; }
}

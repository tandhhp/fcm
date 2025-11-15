using Waffle.Entities;

namespace Waffle.Core.Services.Sources.Args;

public class SourceUpdateArgs : BaseEntity<int>
{
    public string Name { get; set; } = default!;
}

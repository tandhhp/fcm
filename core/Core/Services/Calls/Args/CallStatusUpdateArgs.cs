using Waffle.Entities;

namespace Waffle.Core.Services.Calls.Args;

public class CallStatusUpdateArgs : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}

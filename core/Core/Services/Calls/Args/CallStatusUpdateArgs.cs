using Waffle.Entities;
using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Calls.Args;

public class CallStatusUpdateArgs : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public CallStatusType Type { get; set; }
}

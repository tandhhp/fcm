using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Calls.Args;

public class CallStatusCreateArgs
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public CallStatusType Type { get; set; }
}

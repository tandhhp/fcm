using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Calls.Models;

public class CallStatusSelectOptions : SelectOptions
{
    public CallStatusType? Type { get; set; }
}

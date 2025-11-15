using Waffle.Models;

namespace Waffle.Core.Services.Calls.Models;

public class CallHistoryFilterOptions : FilterOptions
{
    public Guid? ContactId { get; set; }
}

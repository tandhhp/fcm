using Waffle.Models;

namespace Waffle.Core.Services.Calls.Filters;

public class CallStatusDetailFilterOptions : FilterOptions
{
    public Guid? TeleId { get; set; }
    public int? CallStatusId { get; set; }
}

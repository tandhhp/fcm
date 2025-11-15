using Waffle.Models;

namespace Waffle.Core.Services.Histories.Models;

public class HistoryFilterOptions : FilterOptions
{
    public string? Message { get; set; }
    public string? UserName { get; set; }
}

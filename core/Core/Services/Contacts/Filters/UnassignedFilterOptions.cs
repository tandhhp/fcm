using Waffle.Models;

namespace Waffle.Core.Services.Contacts.Filters;

public class UnassignedFilterOptions : FilterOptions
{
    public string? PhoneNumber { get; set; }
    public int? SourceId { get; set; }
}

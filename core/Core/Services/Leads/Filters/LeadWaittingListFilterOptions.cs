using Waffle.Models;

namespace Waffle.Core.Services.Leads.Filters;

public class LeadWaittingListFilterOptions : FilterOptions
{
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public DateTime? EventDate { get; set; }
    public Guid? EventId { get; set; }
}

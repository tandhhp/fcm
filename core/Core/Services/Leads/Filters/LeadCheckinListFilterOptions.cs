using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Leads.Filters;

public class LeadCheckinListFilterOptions : FilterOptions
{
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public DateTime? EventDate { get; set; }
    public Guid? EventId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public LeadStatus? Status { get; set; }
    public string? IdentityNumber { get; set; }
}

using Waffle.Models;

namespace Waffle.Core.Services.Contracts.Filters;

public class ContractFilterOptions : FilterOptions
{
    public string? ContractCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IdentityNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? LeadId { get; set; }
}

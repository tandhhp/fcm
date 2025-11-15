using Waffle.Models;

namespace Waffle.Core.Services.Events.Models;

public class SaleRevenueFilterOptions : FilterOptions
{
    public string? SaleName { get; set; }
    public string? KeyInName { get; set; }
    public string? KeyInPhoneNumber { get; set; }
}

public class KeyInRevenueFilterOptions : FilterOptions
{
    public Guid KeyInId { get; set; }
}
namespace Waffle.Models.Filters;

public class LoyaltyFilterOptions : FilterOptions
{
    public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-1);
    public DateTime ToDate { get; set; } = DateTime.Now;
    public string? Name { get; set; }
    public string? ContractCode { get; set; }
}

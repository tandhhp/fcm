using Waffle.Models;

namespace Waffle.Core.Services.Finances.Bills.Filters;

public class BillFilterOptions : FilterOptions
{
    public Guid? ContractId { get; set; }
}
